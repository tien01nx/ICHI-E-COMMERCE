using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class SupplierService : ISupplierService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    public SupplierService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }
    public Helpers.PagedResult<Supplier> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Suppliers.AsQueryable().Where(u => u.isDeleted == false);
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.SupplierName.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Supplier>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Supplier FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Supplier.Get(u => u.Id == id);
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
    public Supplier Create(Supplier supplier, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var checkSupplier = _unitOfWork.Supplier.Get(u => u.SupplierName == supplier.SupplierName);
        if (checkSupplier != null)
        {
          strMessage = "Mã nhà cung cấp đã tồn tại";
          return null;
        }
        var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email);
        if (checkEmail != null)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null)
        {
          strMessage = "Mã số thuế đã tồn tại";
          return null;
        }
        supplier.CreateBy = "Admin";
        supplier.ModifiedBy = "Admin";
        _unitOfWork.Supplier.Add(supplier);
        _unitOfWork.Save();
        strMessage = "Tạo mới thành công";
        return supplier;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Supplier Update(Supplier supplier, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin nhà cung cấp
        var data = _unitOfWork.Supplier.Get(u => u.Id == supplier.Id);
        if (data == null)
        {
          strMessage = "Nhà cung cấp không tồn tại";
          return null;
        }
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkSupplier = _unitOfWork.Supplier.Get(u => u.SupplierName == supplier.SupplierName);
        if (checkSupplier != null && checkSupplier.Id != supplier.Id)
        {
          strMessage = "Mã nhà cung cấp đã tồn tại";
          return null;
        }
        // kiểm tra email nhà cung cấp đã tồn tại chưa
        var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email);
        if (checkEmail != null && checkEmail.Id != supplier.Id)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
        var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != supplier.Id)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        // kiêm tra mã số thueé
        var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null && checkTaxCode.Id != supplier.Id)
        {
          strMessage = "Mã số thuế đã tồn tại";
          return null;
        }
        supplier.ModifiedBy = "Admin";
        _unitOfWork.Supplier.Update(supplier);
        _unitOfWork.Save();
        strMessage = "Cập nhật thành công";
        return supplier;
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
        var data = _unitOfWork.Supplier.Get(u => u.Id == id && u.isDeleted);
        if (data == null)
        {
          strMessage = "Nhà cung cấp không tồn tại";
          return false;
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Supplier.Update(data);
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
