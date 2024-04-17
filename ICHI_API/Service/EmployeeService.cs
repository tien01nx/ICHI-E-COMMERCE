using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static ICHI_API.Helpers.Constants;


namespace ICHI_API.Service
{
  public class EmployeeService : IEmployeeService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmployeeService(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _webHostEnvironment = webHostEnvironment;
      _db = pcsApiContext;
    }

    public Helpers.PagedResult<Employee> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Employees.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Employee>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Employee FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          throw new BadRequestException(EMPLOYEENOTFOUND);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Employee Create(Employee model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkEmail = _unitOfWork.Employee.Get(u => u.UserId == model.UserId);
        if (checkEmail != null)
        {
          throw new BadRequestException(EMAILEXIST);
        }
        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = PHONENUMBEREXISTCUSTOMER;
          return null;
        }
        model.CreateBy = "Admin";
        model.ModifiedBy = "Admin";
        _unitOfWork.Employee.Add(model);
        _unitOfWork.Save();
        strMessage = CREATECUSTOMERSUCCESS;
        return model;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Employee Update(Employee model, IFormFile? file, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin Nhân viên
        var data = _unitOfWork.Employee.Get(u => u.Id == model.Id);
        if (data == null)
        {
          throw new BadRequestException(EMPLOYEENOTFOUND);
        }
        // kiểm tra email Nhân viên đã tồn tại chưa
        var checkEmail = _unitOfWork.User.Get(u => u.Email == model.UserId);
        if (checkEmail != null && checkEmail.Email != model.UserId)
        {
          throw new BadRequestException(EMAILEXIST);
        }
        // kiểm tra số điện thoại Nhân viên đã tồn tại chưa
        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != model.Id)
        {
          strMessage = PHONENUMBEREXISTCUSTOMER;
          return null;
        }
        // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
        // lấy đường dẫn ảnh file cũ
        if (file != null)
        {
          var user = _unitOfWork.User.Get(x => x.Email == data.UserId);
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
        model.ModifiedBy = "Admin";
        _unitOfWork.Employee.Update(model);
        _unitOfWork.Save();
        strMessage = UPDATEEMPLOYEESUCCESS;
        return model;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public bool Delete(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          throw new BadRequestException(EMPLOYEENOTFOUND);
        }

        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Employee.Update(data);
        _unitOfWork.Save();
        strMessage = DELETEEMPLOYEESUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
