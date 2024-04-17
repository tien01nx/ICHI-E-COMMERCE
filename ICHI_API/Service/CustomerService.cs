using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
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
        throw;
      }
    }

    public Customer FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Customer.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          throw new BadRequestException(Constants.CUSTOMERNOTFOUND);
        }
        return data;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(Constants.EMAILEXIST);
        }
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null)
        {
          throw new BadRequestException(Constants.PHONENUMBEREXISTCUSTOMER);
        }
        //model.CreateBy = "Admin";
        //model.ModifiedBy = "Admin";
        _unitOfWork.Customer.Add(model);
        _unitOfWork.Save();
        strMessage = Constants.CREATECUSTOMERSUCCESS;
        return model;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(Constants.EMAILEXIST);
        }
        // kiểm tra số điện thoại khách hàng đã tồn tại chưa
        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != customer.Id)
        {
          throw new BadRequestException(Constants.PHONENUMBEREXISTCUSTOMER);
        }
        // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
        // lấy đường dẫn ảnh file cũ
        if (file != null)
        {
          var user = _unitOfWork.User.Get(x => x.Email == customer.UserId);
          string oldFile = user.Avatar;
          user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
          //user.ModifiedBy = "Admin";
          //user.ModifiedDate = DateTime.Now;
          _unitOfWork.User.Update(user);
          _unitOfWork.Save();
          // xóa file cũ
          if (oldFile != AppSettings.AvatarDefault)
          {
            ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
          }
        }

        customer.ModifiedBy = "Admin";
        _unitOfWork.Customer.Update(customer);
        _unitOfWork.Save();
        strMessage = Constants.UPDATECUSTOMERSUCCESS;
        return customer;
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
        var data = _unitOfWork.Customer.Get(u => u.Id == id);
        if (data == null)
        {
          throw new BadRequestException(Constants.CUSTOMERNOTFOUND);
        }
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Customer.Update(data);
        _unitOfWork.Save();
        strMessage = Constants.DELETECUSTOMERSUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
