using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
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
        var query = _db.Promotions.AsQueryable().Where(u => u.isDeleted == false);
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
          strMessage = "Nhà cung cấp không tồn tại";
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
    public Promotion Create(Promotion Promotion, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionCode == Promotion.PromotionCode);
        if (checkPromotion != null)
        {
          strMessage = "Mã nhà cung cấp đã tồn tại";
          return null;
        }

        Promotion.CreateBy = "Admin";
        Promotion.ModifiedBy = "Admin";
        _unitOfWork.Promotion.Add(Promotion);
        _unitOfWork.Save();
        strMessage = "Tạo mới thành công";
        return Promotion;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Promotion Update(Promotion Promotion, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin nhà cung cấp
        var data = _unitOfWork.Promotion.Get(u => u.Id == Promotion.Id);
        if (data == null)
        {
          strMessage = "Nhà cung cấp không tồn tại";
          return null;
        }
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionCode == Promotion.PromotionCode);
        if (checkPromotion != null && checkPromotion.Id != Promotion.Id)
        {
          strMessage = "Mã nhà cung cấp đã tồn tại";
          return null;
        }

        Promotion.ModifiedBy = "Admin";
        _unitOfWork.Promotion.Update(Promotion);
        _unitOfWork.Save();
        strMessage = "Cập nhật nhà cung cấp thành công";
        return Promotion;
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
          strMessage = "Nhà cung cấp không tồn tại";
          return false;
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Promotion.Update(data);
        _unitOfWork.Save();
        strMessage = "Xóa nhà cung cấp thành công";
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
