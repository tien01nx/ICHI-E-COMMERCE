using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


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
        Promotion promotion = new Promotion();
        promotion.PromotionName = model.PromotionName;
        promotion.StartTime = model.StartTime;
        promotion.EndTime = model.EndTime;
        promotion.Discount = model.Discount;
        promotion.isActive = model.isActive;
        promotion.isDeleted = model.isDeleted;
        promotion.CreateBy = "Admin";
        promotion.ModifiedBy = "Admin";
        promotion.CreateDate = DateTime.Now;
        promotion.ModifiedDate = DateTime.Now;
        _unitOfWork.Promotion.Add(promotion);
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
              continue;
            }

            item.PromotionId = promotion.Id;
            item.ProductId = item.ProductId;
            item.Quantity = item.Quantity;
            item.UsedCodesCount = item.UsedCodesCount;
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
        var data = _unitOfWork.Promotion.Get(u => u.Id == model.Id);
        if (data == null)
        {
          strMessage = "Có lỗi xảy ra";
          return null;
        }
        // kiểm tra xem mã chương trình khuyến mãi đã tồn tại chưa
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionName == model.PromotionName);
        if (checkPromotion != null && checkPromotion.Id != model.Id)
        {
          strMessage = "Tên chương trình khuyến mãi đã tồn tại";
          return null;
        }
        Promotion promotion = new Promotion();
        promotion.Id = model.Id;
        promotion.PromotionName = model.PromotionName;
        promotion.Discount = model.Discount;
        promotion.StartTime = model.StartTime;
        promotion.EndTime = model.EndTime;
        promotion.isActive = model.isActive;
        promotion.isDeleted = model.isDeleted;
        promotion.ModifiedBy = "Admin";
        _unitOfWork.Promotion.Update(promotion);
        _unitOfWork.Save();
        var promotionDetails = _unitOfWork.PromotionDetail.GetAll(u => u.PromotionId == model.Id, "Product").AsQueryable();
        var promotionDetailDelete = promotionDetails.Where(x => !model.PromotionDetails.Select(y => y.Id).Contains(x.Id)).ToList();
        var promotionDetailUpdate = promotionDetails.Where(x => !model.PromotionDetails.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
        var promotionDetailNew = model.PromotionDetails.Where(x => x.Id == 0).ToList();
        _unitOfWork.PromotionDetail.RemoveRange(promotionDetailDelete);
        List<int> existingProductIds = new List<int>();
        // thực hiện tạo mới product thêm vào chi tiết chương trình khuyến mãi
        if (model.PromotionDetails != null && promotionDetailNew.Count > 0)
        {
          foreach (var itemAdd in promotionDetailNew)
          {
            var productPromotionDetail = _unitOfWork.PromotionDetail.GetAll(u => u.ProductId == itemAdd.ProductId, "Promotion").AsQueryable();
            if (CheckProductPromotion(productPromotionDetail, model.StartTime, model.EndTime) == false)
            {
              existingProductIds.Add(itemAdd.ProductId);
              continue;
            }

            itemAdd.PromotionId = model.Id;
            itemAdd.ProductId = itemAdd.ProductId;
            itemAdd.Quantity = itemAdd.Quantity;
            itemAdd.UsedCodesCount = itemAdd.UsedCodesCount;
            itemAdd.CreateBy = "Admin";
            itemAdd.ModifiedBy = "Admin";
            _unitOfWork.PromotionDetail.Add(itemAdd);
          }
        }

        // update theo danh sách sản phẩm
        if (promotionDetailUpdate.Count > 0)
        {
          foreach (var itemUpdate in promotionDetailUpdate)
          {
            var newProductId = model.PromotionDetails.FirstOrDefault(x => x.ProductId != itemUpdate.ProductId)?.ProductId;

            if (newProductId != null)
            {

              var productPromotionDetail = _unitOfWork.PromotionDetail.GetAll(u => u.ProductId == newProductId, "Promotion").AsQueryable();
              if (CheckProductPromotion(productPromotionDetail, model.StartTime, model.EndTime) == false)
              {
                existingProductIds.Add(newProductId.Value);
                continue;
              }
              itemUpdate.ProductId = newProductId.Value;
              itemUpdate.Quantity = model.PromotionDetails.FirstOrDefault(x => x.ProductId == newProductId.Value).Quantity;
              itemUpdate.UsedCodesCount = model.PromotionDetails.FirstOrDefault(x => x.ProductId == newProductId.Value).UsedCodesCount;
              itemUpdate.ModifiedBy = "Admin";
              itemUpdate.ModifiedDate = DateTime.Now;
            }
          }
        }

        if (existingProductIds.Any())
        {
          strMessage = $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";
          return null;
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
        //data.isDeleted = true;
        //data.ModifiedDate = DateTime.Now;
        //_unitOfWork.Promotion.Update(data);
        _unitOfWork.Promotion.Remove(data);
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

    // check hiện tại chương trình khuyến mãi có hoạt động không

    public IEnumerable<PromotionDetail> CheckPromotionActive()
    {
      try
      {
        DateTime toDay = DateTime.Today.AddSeconds(-1);
        var data = _unitOfWork.Promotion.GetAll(u => u.isActive == true && u.isDeleted == false && u.EndTime >= toDay).AsQueryable();
        var promotionDetails = _unitOfWork.PromotionDetail.GetAll(u => u.Promotion.isActive == true && u.Promotion.isDeleted == false && u.UsedCodesCount < u.Quantity, "Product").AsQueryable();
        List<PromotionDetail> promotionDetailList = new List<PromotionDetail>();
        foreach (var item in data)
        {
          var promotionDetail = promotionDetails.Where(u => u.PromotionId == item.Id).ToList();
          promotionDetailList.AddRange(promotionDetail);
        }
        return promotionDetailList;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

  }
}
