using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CustomerService : ICustomerService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CustomerService(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _webHostEnvironment = webHostEnvironment;
      _db = pcsApiContext;
    }

    public Helpers.PagedResult<Customer> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Customers.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
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

    public Customer Create(Customer model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkEmail = _unitOfWork.Customer.Get(u => u.UserId == model.UserId);
        if (checkEmail != null)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        model.CreateBy = "Admin";
        model.ModifiedBy = "Admin";
        _unitOfWork.Customer.Add(model);
        _unitOfWork.Save();
        strMessage = "Tạo mới thành công";
        return model;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Customer Update(Customer customer, IFormFile? file, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        // kiểm tra email khách hàng đã tồn tại chưa
        var checkEmail = _unitOfWork.User.Get(u => u.Email == customer.UserId);
        if (checkEmail != null && checkEmail.Email != customer.UserId)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        // kiểm tra số điện thoại khách hàng đã tồn tại chưa
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != customer.Id)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
        // lấy đường dẫn ảnh file cũ
        if (file != null)
        {
          //var oldFile = data.Avatar;
          //// lưu file mới
          //data.Avatar = FileHelper.SaveFile(file, _webHostEnvironment, "Customer");
          //// xóa file cũ
          //FileHelper.DeleteFile(oldFile, _webHostEnvironment);
          var user = _unitOfWork.User.Get(x => x.Email == customer.UserId);
          string oldFile = user.Avatar;
          user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
          user.ModifiedBy = "Admin";
          user.ModifiedDate = DateTime.Now;
          _unitOfWork.User.Update(user);
          _unitOfWork.Save();
          // xóa file cũ
          if (oldFile != AppSettings.AvatarDefault)
          {
            ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
          }
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
        var data = _unitOfWork.Customer.Get(u => u.Id == id && !u.isDeleted);
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
