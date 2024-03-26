using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using static System.Net.Mime.MediaTypeNames;


namespace ICHI_API.Service
{
  public class EmployeeService : IEmployeeService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmployeeService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _webHostEnvironment = webHostEnvironment;
      _db = pcsApiContext;
    }

    public EmployeeService(IUnitOfWork unitOfWork, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }

    public Helpers.PagedResult<Employee> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        //var query = _db.Employees.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
        var query = _unitOfWork.Employee.GetAll(u => !u.isDeleted).OrderByDescending(u => u.ModifiedDate).AsQueryable();


        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name) || e.PhoneNumber.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Employee>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Employee FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Employee.Get(u => u.Id == id);
        if (data == null)
        {
          strMessage = "Nhân viên không tồn tại";
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

    public Employee Create(Employee customer, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkEmail = _unitOfWork.Employee.Get(u => u.Email == customer.Email);
        if (checkEmail != null)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == customer.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        customer.CreateBy = "Admin";
        customer.ModifiedBy = "Admin";
        _unitOfWork.Employee.Add(customer);
        _unitOfWork.Save();
        strMessage = "Tạo mới nhân viên thành công";
        return customer;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Employee Update(Employee employee, IFormFile? file, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();

        var existingEmployee = _unitOfWork.Employee.Get(u => u.Id == employee.Id, tracked: false);
        if (existingEmployee == null)
        {
          strMessage = "Nhân viên không tồn tại";
          return null;
        }

        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == employee.PhoneNumber, tracked: false);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }

        if (file != null)
        {
          var user = _unitOfWork.User.Get(x => x.Email == existingEmployee.UserId, tracked: true);
          string oldFile = user.Avatar;
          user.ModifiedBy = "Admin";
          user.ModifiedDate = DateTime.Now;
          _unitOfWork.User.Update(user);
          _unitOfWork.Save();
          if (oldFile != AppSettings.AvatarDefault)
          {
            //ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
          }
        }

        existingEmployee.Address = employee.Address;
        existingEmployee.Avatar = employee.Avatar;
        existingEmployee.Birthday = employee.Birthday;
        existingEmployee.FullName = employee.FullName;
        existingEmployee.PhoneNumber = employee.PhoneNumber;
        existingEmployee.ModifiedBy = "Admin";
        existingEmployee.ModifiedDate = DateTime.Now;

        _unitOfWork.Employee.Update(existingEmployee);
        _unitOfWork.Save();

        _unitOfWork.Commit();

        strMessage = "Cập nhật nhân viên thành công";
        return existingEmployee;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        _unitOfWork.Rollback();
        return null;
      }
    }



    public bool Delete(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted, tracked: true);
        if (data == null)
        {
          strMessage = "Nhân viên không tồn tại";
          return false;
        }

        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Employee.Update(data);
        _unitOfWork.Save();
        strMessage = "Xóa nhân viên thành công";
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
