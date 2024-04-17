using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Dynamic.Core;
using static ICHI_API.Helpers.Constants;

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
          query = query.Where(e => e.SupplierName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Supplier>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(SUPPLIERNOTFOUND);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(SUPPLIEREXIST);
        }
        var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email);
        if (checkEmail != null)
        {
          throw new BadRequestException(EMAILEXIST);
        }
        var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null)
        {
          throw new BadRequestException(PHONENUMBEREXIST);
        }
        var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null)
        {
          throw new BadRequestException(TAXCODEEXIST);
        }
        supplier.CreateBy = "Admin";
        supplier.ModifiedBy = "Admin";
        _unitOfWork.Supplier.Add(supplier);
        _unitOfWork.Save();
        strMessage = ADDSUPPLIERSUCCESS;
        return supplier;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(SUPPLIERNOTFOUND);
        }
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkSupplier = _unitOfWork.Supplier.Get(u => u.SupplierName == supplier.SupplierName);
        if (checkSupplier != null && checkSupplier.Id != supplier.Id)
        {
          throw new BadRequestException(SUPPLIEREXIST);
        }
        // kiểm tra email nhà cung cấp đã tồn tại chưa
        var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email);
        if (checkEmail != null && checkEmail.Id != supplier.Id)
        {
          throw new BadRequestException(EMAILEXIST);
        }
        // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
        var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != supplier.Id)
        {
          throw new BadRequestException(PHONENUMBEREXIST);
        }
        // kiêm tra mã số thueé
        var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null && checkTaxCode.Id != supplier.Id)
        {
          throw new BadRequestException(TAXCODEEXIST);
        }
        supplier.ModifiedBy = "Admin";
        _unitOfWork.Supplier.Update(supplier);
        _unitOfWork.Save();
        strMessage = UPDATESUPPLIERSUCCESS;
        return supplier;
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
        var data = _unitOfWork.Supplier.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          throw new BadRequestException(SUPPLIERNOTFOUND);
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Supplier.Update(data);
        _unitOfWork.Save();
        strMessage = DELETESUPPLIERSUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
