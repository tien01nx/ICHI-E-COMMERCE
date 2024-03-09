using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CustomerService : ICustomerService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    public CustomerService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }
    public Helpers.PagedResult<Customer> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Customers.AsQueryable().Where(u => u.isDeleted == false);
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Customer>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Customer FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Customer.Get(u => u.Id == id);
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
    public Customer Create(Customer customer, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkEmail = _unitOfWork.Customer.Get(u => u.Email == customer.Email);
        if (checkEmail != null)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        customer.CreateBy = "Admin";
        customer.ModifiedBy = "Admin";
        _unitOfWork.Customer.Add(customer);
        _unitOfWork.Save();
        strMessage = "Tạo mới thành công";
        return customer;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Customer Update(Customer customer, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin nhà cung cấp
        var data = _unitOfWork.Customer.Get(u => u.Id == customer.Id);
        if (data == null)
        {
          strMessage = "Nhà cung cấp không tồn tại";
          return null;
        }
        // kiểm tra email nhà cung cấp đã tồn tại chưa
        var checkEmail = _unitOfWork.User.Get(u => u.Email == customer.Email);
        if (checkEmail != null && checkEmail.Id != customer.Id)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != customer.Id)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        // kiêm tra mã số thueé
        customer.ModifiedBy = "Admin";
        _unitOfWork.Customer.Update(customer);
        _unitOfWork.Save();
        strMessage = "Cập nhật thành công";
        return customer;
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
        var data = _unitOfWork.Customer.Get(u => u.Id == id && u.isDeleted);
        if (data == null)
        {
          strMessage = "khách hàng không tồn tại";
          return false;
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Customer.Update(data);
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
