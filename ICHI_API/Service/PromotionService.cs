using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ICHI_API.Service
{
  public class PromotionService : IPromotionService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;

    public PromotionService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }

    public Helpers.PagedResult<Promotion> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        ////var query = _db.Promotions.AsQueryable().Where(u => u.isDeleted == false);
        var query = _unitOfWork.Promotion.GetAll(u => u.isDeleted == false).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.PromotionName.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Promotion>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public PromotionDTO FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var dataPromotion = _unitOfWork.Promotion.Get(u => u.Id == id);
        if (dataPromotion == null)
        {
          strMessage = "Chương trình khuyến mãi không tồn tại";
          return null;
        }

        PromotionDTO model = new PromotionDTO
        {
          Promotion = dataPromotion,
          PromotionDetails = _unitOfWork.PromotionDetail.GetAll(u => u.PromotionId == dataPromotion.Id, "Product")
        };

        return model;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public PromotionDTO Create(PromotionDTO model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionName == model.PromotionName);
        if (checkPromotion != null)
        {
          strMessage = "Mã chương trình khuyến mãi đã tồn tại";
          return null;
        }

        if (model.StartTime < DateTime.Today)
        {
          strMessage = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại";
          return null;
        }

        if (model.EndTime <= DateTime.Today)
        {
          strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày hiện tại";
          return null;
        }

        if (model.StartTime > model.EndTime)
        {
          strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu";
          return null;
        }

        model.CreateBy = "Admin";
        model.ModifiedBy = "Admin";

        _unitOfWork.Promotion.Add(model.Promotion);
        _unitOfWork.Save();

        // thực hiện thêm chi tiết chương trình khuyến mãi
        if (model.PromotionDetails != null && model.PromotionDetails.Count() > 0)
        {
          List<int> existingProductIds = new List<int>();
          foreach (var item in model.PromotionDetails)
          {
            var promotionDetails = _unitOfWork.PromotionDetail.GetAll(u => u.ProductId == item.ProductId, "Promotion").AsQueryable();

            if (CheckProductPromotion(promotionDetails, model.StartTime, model.EndTime) == false)
            {
              existingProductIds.Add(item.ProductId);
            }

            item.PromotionId = model.Promotion.Id;
            item.ProductId = item.ProductId;
            item.CreateBy = "Admin";
            item.ModifiedBy = "Admin";
            _unitOfWork.PromotionDetail.Add(item);
          }
          if (existingProductIds.Any())
          {
            strMessage = $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";
            return null;
          }
          _unitOfWork.Save();
        }

        strMessage = "Tạo mới chương trình khuyến mãi thành công";
        _unitOfWork.Commit();
        return model;
      }
      catch (Exception ex)
      {
        _unitOfWork.Rollback();
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }


    public PromotionDTO Update(PromotionDTO model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        // lấy thông tin chương trình khuyến mãi
        var data = _unitOfWork.Promotion.Get(u => u.Id == model.Promotion.Id);
        if (data == null)
        {
          strMessage = "Có lỗi xảy ra";
          return null;
        }
        // kiểm tra xem mã chương trình khuyến mãi đã tồn tại chưa
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionName == model.Promotion.PromotionName);
        if (checkPromotion != null && checkPromotion.Id != model.Promotion.Id)
        {
          strMessage = "Tên chương trình khuyến mãi đã tồn tại";
          return null;
        }

        model.Promotion.ModifiedBy = "Admin";
        _unitOfWork.Promotion.Update(model.Promotion);
        _unitOfWork.Save();

        var promotionDetails = _unitOfWork.PromotionDetail.GetAll(u => u.PromotionId == model.Promotion.Id, "Product").AsQueryable();
        var promotionDetailDelete = promotionDetails.Where(x => !model.PromotionDetails.Select(y => y.Id).Contains(x.Id)).ToList();
        var promotionDetailNew = model.PromotionDetails.Where(x => x.Id == 0).ToList();
        _unitOfWork.PromotionDetail.RemoveRange(promotionDetailDelete);

        // thực hiện cập nhật chi tiết chương trình khuyến mãi
        if (model.PromotionDetails != null && promotionDetailNew.Count > 0)
        {
          List<int> existingProductIds = new List<int>();
          foreach (var itemAdd in promotionDetailNew)
          {
            var productPromotionDetail = _unitOfWork.PromotionDetail.GetAll(u => u.ProductId == itemAdd.Id, "Product").AsQueryable();
            if (CheckProductPromotion(productPromotionDetail, model.StartTime, model.EndTime) == false)
            {
              existingProductIds.Add(itemAdd.ProductId);
            }

            itemAdd.PromotionId = model.Promotion.Id;
            itemAdd.ProductId = itemAdd.ProductId;
            itemAdd.CreateBy = "Admin";
            itemAdd.ModifiedBy = "Admin";
            _unitOfWork.PromotionDetail.Add(itemAdd);
          }

          if (existingProductIds.Any())
          {
            strMessage = $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";
            return null;
          }


        }

        strMessage = "Cập nhật chương trình khuyến mãi thành công";
        _unitOfWork.Save();
        _unitOfWork.Commit();
        return model;
      }
      catch (Exception ex)
      {
        _unitOfWork.Rollback();
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public bool Delete(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Promotion.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          strMessage = "Mã chương trình khuyến mãi không tồn tại";
          return false;
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Promotion.Update(data);
        _unitOfWork.Save();
        strMessage = "Xóa chương trình khuyến mãi thành công";
        return true;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return false;
      }
    }

    public bool CheckProductPromotion(IQueryable<PromotionDetail> data, DateTime StartTime, DateTime EndTime)
    {
      try
      {
        bool isOutsidePromotion = !data.Any(u =>
             (StartTime >= u.Promotion.StartTime && StartTime <= u.Promotion.EndTime) ||
             (EndTime >= u.Promotion.StartTime && EndTime <= u.Promotion.EndTime));
        return isOutsidePromotion;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
