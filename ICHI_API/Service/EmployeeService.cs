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

    public Employee Create(Employee model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {

        var checkEmail = _unitOfWork.Employee.Get(u => u.UserId == model.UserId);
        if (checkEmail != null)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null)
        {
          strMessage = "Số điện thoại đã tồn tại";
          return null;
        }
        model.CreateBy = "Admin";
        model.ModifiedBy = "Admin";
        _unitOfWork.Employee.Add(model);
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

    public Employee Update(Employee model, IFormFile? file, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin Nhân viên
        var data = _unitOfWork.Employee.Get(u => u.Id == model.Id);
        if (data == null)
        {
          strMessage = "Nhân viên không tồn tại";
          return null;
        }
        // kiểm tra email Nhân viên đã tồn tại chưa
        var checkEmail = _unitOfWork.User.Get(u => u.Email == model.UserId);
        if (checkEmail != null && checkEmail.Email != model.UserId)
        {
          strMessage = "Email đã tồn tại";
          return null;
        }
        // kiểm tra số điện thoại Nhân viên đã tồn tại chưa
        var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
        if (checkPhone != null && checkPhone.Id != model.Id)
        {
          strMessage = "Số điện thoại đã tồn tại";
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
        strMessage = "Cập nhật nhân viên thành công";
        return model;
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
        var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
        if (data == null)
        {
          strMessage = "Nhân viên không tồn tại";
          return false;
        }

        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Employee.Update(data);
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
