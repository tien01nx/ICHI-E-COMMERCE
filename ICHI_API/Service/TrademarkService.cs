using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Dynamic.Core;
using static ICHI_API.Helpers.Constants;


namespace ICHI_API.Service
{
  public class TrademarkService : ITrademarkService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TrademarkService(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _webHostEnvironment = webHostEnvironment;
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
          query = query.Where(e => e.TrademarkName.Contains(name.Trim()));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Trademark>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(TRADEMARKNOTFOUND);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(TRADEMARKEXIST);
        }
        trademark.CreateBy = "Admin";
        trademark.ModifiedBy = "Admin";
        _unitOfWork.Trademark.Add(trademark);
        _unitOfWork.Save();
        strMessage = ADDTRADEMARKSUCCESS;
        return trademark;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Trademark Update(Trademark trademark, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin thương hiệu
        var data = _unitOfWork.Trademark.Get(u => u.Id == trademark.Id);
        if (data == null)
        {
          throw new BadRequestException(TRADEMARKNOTFOUND);
        }
        // kiểm tra số điện thoại thương hiệu đã tồn tại chưa
        var trademarkName = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim());
        if (trademarkName != null && trademarkName.Id != trademark.Id)
        {
          throw new BadRequestException(TRADEMARKEXIST);
        }
        // kiêm tra mã số thueé
        trademark.ModifiedBy = "Admin";
        _unitOfWork.Trademark.Update(trademark);
        _unitOfWork.Save();
        strMessage = UPDATETRADEMARKSUCCESS;
        return trademark;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public bool Delete(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Trademark.Get(u => u.Id == id);
        if (data == null)
        {
          throw new BadRequestException(TRADEMARKNOTFOUND);
        }
        _unitOfWork.Trademark.Remove(data);
        _unitOfWork.Save();
        strMessage = DELETETRADEMARKSUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
