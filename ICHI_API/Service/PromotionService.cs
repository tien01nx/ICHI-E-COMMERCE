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

    public Promotion FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Promotion.Get(u => u.Id == id);
        if (data == null)
        {
          strMessage = "Mã chương trình khuyến mãi không tồn tại";
          return null;
        }
        return data;
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
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionName == model.Promotion.PromotionName);
        if (checkPromotion != null)
        {
          strMessage = "Mã chương trình khuyến mãi đã tồn tại";
          return null;
        }
        if(model.Promotion.StartTime < DateTime.Today)
        {
          strMessage = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại";
          return null;
        }
        if (model.Promotion.EndTime <= DateTime.Today)
        {
          strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày hiện tại";
          return null;
        }
        if(model.Promotion.StartTime > model.Promotion.EndTime)
        {
          strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu";
          return null;
        }



        model.Promotion.CreateBy = "Admin";
        model.Promotion.ModifiedBy = "Admin";
        _unitOfWork.Promotion.Add(model.Promotion);
        _unitOfWork.Save();

        // thực hiện thêm chi tiết chương trình khuyến mãi
        if (model.PromotionDetails != null && model.PromotionDetails.Count() > 0)
        {
          string strError = string.Empty;
          List<int> existingProductIds = new List<int>();
          foreach (var item in model.PromotionDetails)
          {
            var product = _unitOfWork.Product.Get(u => u.Id == item.ProductId);
            if (product == null)
            {
              strMessage = "Sản phẩm không tồn tại";
              return null;
            }
            var checkExistProduct = _db.PromotionDetails.Include(u => u.Promotion).Where(u => u.ProductId == item.ProductId
                           && 
                           (
                           (u.Promotion.StartTime >= model.Promotion.StartTime
                           && u.Promotion.EndTime >= model.Promotion.EndTime)
                           || (u.Promotion.StartTime <= model.Promotion.StartTime
                           && u.Promotion.EndTime >= model.Promotion.EndTime
                           )
                           || (u.Promotion.StartTime >= model.Promotion.StartTime
                           && u.Promotion.StartTime >= model.Promotion.EndTime)
                          || (u.Promotion.EndTime <= model.Promotion.StartTime
                           && u.Promotion.EndTime <= model.Promotion.EndTime)));
            if (checkExistProduct.Count() > 0)
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
            //string pattern = @"Id:\s*([\d,]+)\s*:"; // Biểu thức chính quy để tìm chuỗi số và dấu phẩy nằm giữa "Id:" và ":"

            //Match match = Regex.Match(strMessage, pattern);

            //if (match.Success)
            //{
            //  string numbersString = match.Groups[1].Value;
            //}
            //else
            //{
            //  Console.WriteLine("Không tìm thấy chuỗi số.");
            //}

            return null;
          }
          _unitOfWork.Save();
        }

        strMessage = "Tạo mới thành công";
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

        // lấy thông tin chương trình khuyến mãi
        var data = _unitOfWork.Promotion.Get(u => u.Id == model.Promotion.Id);
        if (data == null)
        {
          strMessage = "Mã chương trình khuyến mãi không tồn tại";
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
        var promotionDetails = _unitOfWork.PromotionDetail.GetAll(u=> u.PromotionId == model.Promotion.Id);
        // thực hiện cập nhật chi tiết chương trình khuyến mãi
        if (model.PromotionDetails != null && model.PromotionDetails.Count() > 0)
        {
          foreach (var item in model.PromotionDetails)
          {
            // danh sách sản phẩm trong chương trình khuyến mãi trong db là
            // dách sách sản phẩm trong chương trình khuyến mãi mà thực hiện thêm mới là khi có id =0
            var promotionDetail = model.PromotionDetails.Where(u => u.Id == 0).ToList();
            // danh sách sản phẩm mà thực hiện xóa là 
            var promotionDetailDelete = promotionDetails.Where(u => !model.PromotionDetails.Select(x => x.Id).Contains(u.Id)).ToList();

            // thực hiện thêm mới sản phẩm
            if (promotionDetail != null && promotionDetail.Count() > 0)
            {
              foreach (var itemAdd in promotionDetail)
              {
                var product = _unitOfWork.Product.Get(u => u.Id == itemAdd.ProductId);
                if (product == null)
                {
                  strMessage = "Sản phẩm không tồn tại";
                  return null;
                }
                // thực hiện kiểm tra sản phẩm đã tồn tại trong chương trình khuyến mãi chưa, đảm bảo mã sản phẩm đó phải hết thời hạn trước  đó thì mới cho thêm
                var checkPromotionDetail = _unitOfWork.PromotionDetail.Get(u => u.ProductId == itemAdd.ProductId && u.PromotionId == model.Promotion.Id
                                                                                                       && u.Promotion.EndTime < DateTime.Now.AddDays(1).AddSeconds(-1), "Promotion");
                if (checkPromotionDetail != null)
                {
                  strMessage = "Sản phẩm đã tồn tại trong chương trình khuyến mãi";
                  return null;
                }

                itemAdd.PromotionId = model.Promotion.Id;
                itemAdd.ProductId = itemAdd.ProductId;
                itemAdd.CreateBy = "Admin";
                itemAdd.ModifiedBy = "Admin";
                _unitOfWork.PromotionDetail.Add(itemAdd);
              }
            }

            // thực hiện xóa sản phẩm
            _unitOfWork.PromotionDetail.RemoveRange(promotionDetailDelete);

          }
          _unitOfWork.Save();
        }


        strMessage = "Cập nhật chương trình khuyến mãi thành công";
        return model;
      }
      catch (Exception ex)
      {
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
  }
}
