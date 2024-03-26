using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static System.Net.Mime.MediaTypeNames;


namespace ICHI_API.Service
{
  public class TrademarkService : ITrademarkService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;

    public TrademarkService(IUnitOfWork unitOfWork, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }

    public Helpers.PagedResult<Trademark> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Trademarks.OrderByDescending(u => u.ModifiedDate).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.TrademarkName.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Trademark>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Trademark FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Trademark.Get(u => u.Id == id);
        if (data == null)
        {
          strMessage = "Thương hiệu không tồn tại";
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

    public Trademark Create(Trademark trademark, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkPhone = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim());
        if (checkPhone != null)
        {
          strMessage = "Tên thương hiệu đã tồn tại";
          return null;
        }
        trademark.CreateBy = "Admin";
        trademark.ModifiedBy = "Admin";
        _unitOfWork.Trademark.Add(trademark);
        _unitOfWork.Save();
        strMessage = "Tạo mới thương hiệu thành công";
        return trademark;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Trademark Update(Trademark trademark, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin thương hiệu
        var data = _unitOfWork.Trademark.Get(u => u.Id == trademark.Id, tracked: true);
        if (data == null)
        {
          strMessage = "Thương hiệu không tồn tại";
          return null;
        }
        // kiểm tra số điện thoại thương hiệu đã tồn tại chưa
        var trademarkName = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim(), tracked: true);
        if (trademarkName != null && trademarkName.Id != trademark.Id)
        {
          strMessage = "Tên thương hiệu đã tồn tại";
          return null;
        }
        // kiêm tra mã số thueé
        data.TrademarkName = trademark.TrademarkName;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        trademark.ModifiedBy = "Admin";
        _unitOfWork.Trademark.Update(data);
        _unitOfWork.Save();
        strMessage = "Cập nhật thành công";
        return trademark;
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
        var data = _unitOfWork.Trademark.Get(u => u.Id == id, tracked: true);
        if (data == null)
        {
          strMessage = "Thương hiệu không tồn tại";
          return false;
        }
        _unitOfWork.Trademark.Remove(data);
        _unitOfWork.Save();
        strMessage = "Xóa thành công";
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
