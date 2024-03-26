using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Migrations;
using ICHI_API.Service;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using API.Model;
using ICHI_API.Extension;
using ICHI_API.Model;
using ICHI_CORE.Model;

namespace ICHI_TEST.ServiceTest
{
  public class CustomerServiceTest
  {
    private readonly PcsApiContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerService _customerService;
    private readonly IUserService _userSerivce;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly ISupplierService _supplierService;
    private readonly ITrademarkService _trademarkService;
    private readonly IProductService _productService;
    private readonly ICategoryProductService _categoryProductService;

    #region Customer Server 

    //public class Customer : MasterEntity
    //{
    //  public string UserId { get; set; }

    //  [ForeignKey("UserId")]
    //  [ValidateNever]
    //  public User? User { get; set; }

    //  [Required]
    //  [StringLength(255)]
    //  public string FullName { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(3)]
    //  public string Gender { get; set; } = string.Empty;

    //  public DateTime Birthday { get; set; }

    //  [StringLength(255)]
    //  public string Email { get; set; } = string.Empty;

    //  [StringLength(12)]
    //  public string PhoneNumber { get; set; } = string.Empty;

    //  [StringLength(255)]
    //  public string Address { get; set; } = string.Empty;

    //  public string Avatar { get; set; } = string.Empty;

    //  public bool isActive { get; set; } = false;

    //  public bool isDeleted { get; set; } = false;
    //}
    //public CustomerService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
    //{
    //  _unitOfWork = unitOfWork;
    //  _webHostEnvironment = webHostEnvironment;
    //  _db = pcsApiContext;
    //}

    //public Helpers.PagedResult<Customer> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    //var query = _db.Customers.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
    //    var query = _unitOfWork.Customer.GetAll(u => !u.isDeleted).OrderByDescending(u => u.ModifiedDate).AsQueryable();

    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.FullName.Contains(name) || e.PhoneNumber.Contains(name));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Customer>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Customer FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Customer.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      strMessage = "Khách hàng không tồn tại";
    //      return null;
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Customer Create(Customer customer, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {

    //    var checkEmail = _unitOfWork.Customer.Get(u => u.Email == customer.Email);
    //    if (checkEmail != null)
    //    {
    //      strMessage = "Email đã tồn tại";
    //      return null;
    //    }
    //    var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
    //    if (checkPhone != null)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }

    //    //_db.Entry(customer).State = EntityState.Detached;
    //    customer.CreateBy = "Admin";
    //    customer.ModifiedBy = "Admin";
    //    _unitOfWork.Customer.Add(customer);
    //    _unitOfWork.Save();
    //    strMessage = "Tạo mới thành công";
    //    return customer;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Customer Update(Customer customer, IFormFile? file, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // kiểm tra số điện thoại khách hàng đã tồn tại chưa
    //    var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
    //    if (checkPhone != null)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }
    //    // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
    //    // lấy đường dẫn ảnh file cũ
    //    if (file != null)
    //    {
    //      var user = _unitOfWork.User.GetAll(x => x.Email == customer.Email).FirstOrDefault();
    //      string oldFile = user.Avatar;
    //      //user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
    //      user.ModifiedBy = "Admin";
    //      user.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(user);
    //      _unitOfWork.Save();
    //      // xóa file cũ
    //      if (oldFile != AppSettings.AvatarDefault)
    //      {
    //        //ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
    //      }
    //    }
    //    _db.Entry(customer).State = EntityState.Detached;
    //    customer.ModifiedBy = "Admin";
    //    _unitOfWork.Customer.Update(customer);
    //    _unitOfWork.Save();

    //    strMessage = "Cập nhật thành công";
    //    return customer;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Customer.GetAll(u => u.Id == id && !u.isDeleted).FirstOrDefault();
    //    if (data == null)
    //    {
    //      strMessage = "khách hàng không tồn tại";
    //      return false;
    //    }
    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Customer.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa khách hàng thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}

    #endregion

    // Thực hiện Test CustomerService
    public CustomerServiceTest()
    {
      _context = ContextGenerator.Generator();
      _unitOfWork = new UnitOfWork(_context);
      CreateDataUser();
      CreateDataCustomer();
      CreateDataEmployee();
      CreateDataSupplier();
      CreateDataCategory();
      CreateDataTrademark();
      CreateDataProduct();


      var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
      _customerService = new CustomerService(_unitOfWork, webHostEnvironmentMock.Object, _context);
      _userSerivce = new UserService(_unitOfWork, _context);
      _employeeService = new EmployeeService(_unitOfWork, webHostEnvironmentMock.Object, _context);
      _authService = new AuthService(_unitOfWork);
      _supplierService = new SupplierService(_unitOfWork, _context);
      _trademarkService = new TrademarkService(_unitOfWork, _context);
      _categoryProductService = new CategoryProductService(_unitOfWork, _context);
      _productService = new ProductService(_unitOfWork, _context);
    }
    // fake data Customer
    public void CreateDataCustomer()
    {
      // Arrange
      for (int i = 0; i < 30; i++)
      {
        _context.Customers.Add(new Customer
        {
          Avatar = "Avatar",
          Birthday = DateTime.Now,
          Email = $"Email {i}",
          FullName = $"FullName {i}",
          isDeleted = false,
          UserId = "demo111111@gmail",
          Address = "Address" + i,
          Gender = "Nam",
          CreateDate = DateTime.Now,
          ModifiedDate = DateTime.Now,
          PhoneNumber = "0987316531",
          CreateBy = "Admin",
          ModifiedBy = "Admin"
        });
      }
      _context.SaveChanges();
    }
    // fake data User
    public void CreateDataUser()
    {
      List<User> users = new List<User>();
      users.Add(new User
      {
        Email = "Email 1",
        Avatar = "Avatar",
        CreateBy = "Admin",
        CreateDate = DateTime.Now,
        FailedPassAttemptCount = 0,
        IsLocked = false,
        Password = "123456"
      });
      users.Add(new User
      {
        Email = "Email 2",
        Avatar = "Avatar",
        CreateBy = "Admin",
        CreateDate = DateTime.Now,
        FailedPassAttemptCount = 0,
        IsLocked = false,
        Password = "123456"
      });
      users.Add(new User
      {
        Email = "Email 3",
        Avatar = "Avatar",
        CreateBy = "Admin",
        CreateDate = DateTime.Now,
        FailedPassAttemptCount = 0,
        IsLocked = false,
        Password = "123456"
      });
      users.Add(new User
      {
        Email = "Email 4",
        Avatar = "Avatar",
        CreateBy = "Admin",
        CreateDate = DateTime.Now,
        FailedPassAttemptCount = 0,
        IsLocked = false,
        Password = "123456"
      });
      users.Add(new User
      {
        Email = "Email 6",
        Avatar = "Avatar",
        CreateBy = "Admin",
        CreateDate = DateTime.Now,
        FailedPassAttemptCount = 0,
        IsLocked = false,
        Password = "123456"
      });

      _context.Users.AddRange(users);
      _context.SaveChanges();

    }
    /// <summary>
    /// Test case Kiểm tra lấy danh sách khách hàng 10 bản ghi
    /// </summary>
    [Fact]
    public void GetAllCustomerSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.GetAll("", 10, 1, "asc", "FullName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }
    /// <summary>
    /// Test case  Kiểm tra lấy danh sách sách khachs hàng khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllCustomerFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.GetAll("", 10, 10, "asc", "FullName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(0, result.Items.Count());
    }
    /// <summary>
    /// Test case kiểm tra lấy khách hàng theo id
    /// </summary>
    [Fact]
    public void FindByIdCustomerSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("FullName 0", result.FullName);
    }
    [Fact]
    public void FindByIdCustomerFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Khách hàng không tồn tại", strMessage);
    }
    /// <summary>
    /// Test case kiểm tra tạo mới khách hàng thành công
    /// </summary>
    [Fact]
    public void CreateCustomerSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _customerService.Create(new Customer
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        Email = "Email 30",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "demo2202@gmail.com",
        Gender = "Nam",
        isActive = true,
        PhoneNumber = "098743124"
      }, out strMessage);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Tạo mới thành công", strMessage);

    }
    /// <summary>
    /// Test case kiểm tra tạo mới khách hàng thất bại khi trùng email
    /// </summary>
    [Fact]
    public void CreateCustomerEmailFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _customerService.Create(new Customer
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        Email = "Email 4",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "demo111111@gmail.com"
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Email đã tồn tại", strMessage);
    }
    /// <summary>
    /// Test case kiểm tra tạo mới khách hàng thất bại khi trùng số điện thoại
    /// </summary>
    [Fact]
    public void CreateCustomerPhoneNumberFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _customerService.Create(new Customer
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        Email = "Email 30",
        FullName = "FullName 30",
        isDeleted = false,
        PhoneNumber = "0987316531",
        UserId = "demo111111@gmail.com"
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }
    /// <summary>
    /// test case thực hiện cập nhật khách hàng thành công
    /// </summary>
    [Fact]
    public void UpdateCustomerSuccess()
    {
      string strMessage = string.Empty;

      // Tạo giả lập IFormFile
      var fileMock = new Mock<IFormFile>();
      // Thiết lập các thuộc tính của giả lập
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
      fileMock.Setup(f => f.Length).Returns(1234);

      // Act
      var result = _customerService.Update(new Customer
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        Email = "Email 6",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "demo111111@gmail"
      }, fileMock.Object, out strMessage);
      Assert.NotNull(result);
      Assert.Equal("Cập nhật thành công", strMessage);
    }
    /// <summary>
    /// Test case thực hiện cập nhật khách hàng thất bại khi trùng Số điện thoại
    /// </summary>
    [Fact]
    public void UpdateCustomerPhoneNumberFail()
    {
      string strMessage = string.Empty;

      // Tạo giả lập IFormFile
      var fileMock = new Mock<IFormFile>();
      // Thiết lập các thuộc tính của giả lập
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
      fileMock.Setup(f => f.Length).Returns(1234);

      // Act
      var result = _customerService.Update(new Customer
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        Email = "Email 6",
        FullName = "FullName 30",
        Id = 30,
        PhoneNumber = "0987316531",
        isDeleted = false,
        UserId = "demo111111@gmail"
      }, fileMock.Object, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }
    /// <summary>
    /// Test case thực hiện xóa khách hàng thành công
    /// </summary>
    [Fact]
    public void DeleteCustomerSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.Delete(1, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa khách hàng thành công", strMessage);
    }
    /// <summary>
    /// Test case thực hiện xóa khách hàng thành công
    /// </summary>
    [Fact]
    public void DeleteCustomerFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _customerService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("khách hàng không tồn tại", strMessage);
    }

    #region Employee Server
    //public class Employee : MasterEntity
    //{
    //  public string UserId { get; set; }

    //  [ForeignKey("UserId")]
    //  [ValidateNever]
    //  public User? User { get; set; }

    //  [Required]
    //  [StringLength(255)]
    //  public string FullName { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(3)]
    //  public string Gender { get; set; } = string.Empty;

    //  public DateTime Birthday { get; set; }
    //  [StringLength(255)]
    //  public string Email { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(12)]
    //  public string PhoneNumber { get; set; } = string.Empty;

    //  [StringLength(255)]
    //  public string Address { get; set; } = string.Empty;

    //  public bool isActive { get; set; } = false;

    //  public bool isDeleted { get; set; } = false;

    //  public string Avatar { get; set; } = string.Empty;
    //}

    //public Helpers.PagedResult<Employee> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.Employees.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.FullName.Contains(name) || e.PhoneNumber.Contains(name));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Employee>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Employee FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Employee.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      strMessage = "Nhân viên không tồn tại";
    //      return null;
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Employee Create(Employee customer, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {

    //    var checkEmail = _unitOfWork.Employee.Get(u => u.Email == customer.Email);
    //    if (checkEmail != null)
    //    {
    //      strMessage = "Email đã tồn tại";
    //      return null;
    //    }
    //    var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == customer.PhoneNumber);
    //    if (checkPhone != null)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }
    //    customer.CreateBy = "Admin";
    //    customer.ModifiedBy = "Admin";
    //    _unitOfWork.Employee.Add(customer);
    //    _unitOfWork.Save();
    //    strMessage = "Tạo mới thành công";
    //    return customer;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Employee Update(Employee customer, IFormFile? file, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // lấy thông tin Nhân viên
    //    var data = _unitOfWork.Employee.Get(u => u.Id == customer.Id);
    //    if (data == null)
    //    {
    //      strMessage = "Nhân viên không tồn tại";
    //      return null;
    //    }
    //    // kiểm tra số điện thoại Nhân viên đã tồn tại chưa
    //    var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == customer.PhoneNumber);
    //    if (checkPhone != null && checkPhone.Id != customer.Id)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }
    //    // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
    //    // lấy đường dẫn ảnh file cũ
    //    if (file != null)
    //    {
    //      var user = _unitOfWork.User.Get(x => x.Email == data.Email);
    //      string oldFile = user.Avatar;
    //      user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
    //      user.ModifiedBy = "Admin";
    //      user.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(user);
    //      _unitOfWork.Save();
    //      // xóa file cũ
    //      if (oldFile != AppSettings.AvatarDefault)
    //      {
    //        ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
    //      }
    //    }
    //    customer.ModifiedBy = "Admin";
    //    _unitOfWork.Employee.Update(customer);
    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật thành công";
    //    return customer;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
    //    if (data == null)
    //    {
    //      strMessage = "Nhân viên không tồn tại";
    //      return false;
    //    }

    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Employee.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}
    #endregion

    // thực hiện test EmployeeService

    //fake Data Employee
    public void CreateDataEmployee()
    {
      // Arrange
      for (int i = 1; i < 30; i++)
      {
        _context.Employees.Add(new Employee
        {
          Avatar = "Avatar",
          Birthday = DateTime.Now,
          Email = $"Email {i}",
          PhoneNumber = "0987316531",
          FullName = $"FullName {i}",
          isDeleted = false,
          UserId = "demo111111@gmail",
          Address = "Address" + i,
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case Kiểm tra lấy danh sách nhân viên 10 bản ghi 
    /// </summary>
    [Fact]
    public void GetAllEmployeeSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.GetAll("", 10, 1, "asc", "FullName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }

    /// <summary>
    /// Test case Kiểm tra lấy danh sách nhân viên khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllEmployeeFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.GetAll("", 10, 10, "asc", "FullName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Test case kiểm tra lấy nhân viên theo id
    /// </summary>
    [Fact]
    public void FindByIdEmployeeSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("FullName 1", result.FullName);
    }

    /// <summary>
    /// Test case kiểm tra lấy nhân viên theo id khi không tồn tại
    /// </summary>
    [Fact]
    public void FindByIdEmployeeFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Nhân viên không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case kiểm tra tạo mới nhân viên thành công
    /// </summary>
    [Fact]
    public void CreateEmployeeSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _employeeService.Create(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        PhoneNumber = "098733333",
        Email = "Email 123",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "Email 1"
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Tạo mới nhân viên thành công", strMessage);
    }

    /// <summary>
    /// Test case kiểm tra tạo mới nhân viên thất bại khi trùng email
    /// </summary>
    [Fact]
    public void CreateEmployeeEmailFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _employeeService.Create(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        PhoneNumber = "0987316532",
        Email = "Email 1",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "Email 1"
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Email đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case kiểm tra tạo mới nhân viên khi trùng số điện thoại
    /// </summary>
    [Fact]
    public void CreateEmployeePhoneNumberFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _employeeService.Create(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Birthday = DateTime.Now,
        PhoneNumber = "0987316531",
        Email = "Email 123",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "Email 1"
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }


    /// <summary>
    /// Test case thực hiện xóa nhân viên thành công
    /// </summary>
    [Fact]
    public void DeleteEmployeeSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.Delete(2, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa nhân viên thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa nhân viên thất bại
    /// </summary>
    [Fact]
    public void DeleteEmployeeFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _employeeService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Nhân viên không tồn tại", strMessage);
    }

    #region Supplier Service 

    //public class Supplier : MasterEntity
    //{
    //  [Required]
    //  [StringLength(255)]
    //  public string SupplierName { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(50)]
    //  public string TaxCode { get; set; } = string.Empty;

    //  [StringLength(255)]
    //  public string Address { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(12)]
    //  public string PhoneNumber { get; set; } = string.Empty;

    //  [StringLength(255)]
    //  public string Email { get; set; } = string.Empty;

    //  [StringLength(50)]
    //  public string BankAccount { get; set; } = string.Empty;

    //  [StringLength(100)]
    //  public string BankName { get; set; } = string.Empty;

    //  public string Notes { get; set; } = string.Empty;

    //  public bool isActive { get; set; } = false;

    //  public bool isDeleted { get; set; } = false;
    //}
    //public Helpers.PagedResult<Supplier> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.Suppliers.AsQueryable().Where(u => u.isDeleted == false);
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.SupplierName.Contains(name) || e.PhoneNumber.Contains(name));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Supplier>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Supplier FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Supplier.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      strMessage = "Nhà cung cấp không tồn tại";
    //      return null;
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Supplier Create(Supplier supplier, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var checkSupplier = _unitOfWork.Supplier.Get(u => u.SupplierName == supplier.SupplierName, tracked: true);
    //    if (checkSupplier != null)
    //    {
    //      strMessage = "Mã nhà cung cấp đã tồn tại";
    //      return null;
    //    }
    //    var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email, tracked: true);
    //    if (checkEmail != null)
    //    {
    //      strMessage = "Email đã tồn tại";
    //      return null;
    //    }
    //    var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber, tracked: true);
    //    if (checkPhone != null)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }
    //    var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode, tracked: true);
    //    if (checkTaxCode != null)
    //    {
    //      strMessage = "Mã số thuế đã tồn tại";
    //      return null;
    //    }
    //    supplier.CreateBy = "Admin";
    //    supplier.ModifiedBy = "Admin";
    //    _unitOfWork.Supplier.Add(supplier);
    //    _unitOfWork.Save();
    //    strMessage = "Tạo mới thành công";
    //    return supplier;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Supplier Update(Supplier supplier, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // lấy thông tin nhà cung cấp
    //    var data = _unitOfWork.Supplier.Get(u => u.Id == supplier.Id);
    //    if (data == null)
    //    {
    //      strMessage = "Nhà cung cấp không tồn tại";
    //      return null;
    //    }
    //    // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
    //    var checkSupplier = _unitOfWork.Supplier.Get(u => u.SupplierName == supplier.SupplierName);
    //    if (checkSupplier != null && checkSupplier.Id != supplier.Id)
    //    {
    //      strMessage = "Mã nhà cung cấp đã tồn tại";
    //      return null;
    //    }
    //    // kiểm tra email nhà cung cấp đã tồn tại chưa
    //    var checkEmail = _unitOfWork.Supplier.Get(u => u.Email == supplier.Email);
    //    if (checkEmail != null && checkEmail.Id != supplier.Id)
    //    {
    //      strMessage = "Email đã tồn tại";
    //      return null;
    //    }
    //    // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
    //    var checkPhone = _unitOfWork.Supplier.Get(u => u.PhoneNumber == supplier.PhoneNumber);
    //    if (checkPhone != null && checkPhone.Id != supplier.Id)
    //    {
    //      strMessage = "Số điện thoại đã tồn tại";
    //      return null;
    //    }
    //    // kiêm tra mã số thueé
    //    var checkTaxCode = _unitOfWork.Supplier.Get(u => u.TaxCode == supplier.TaxCode);
    //    if (checkTaxCode != null && checkTaxCode.Id != supplier.Id)
    //    {
    //      strMessage = "Mã số thuế đã tồn tại";
    //      return null;
    //    }
    //    supplier.ModifiedBy = "Admin";
    //    _unitOfWork.Supplier.Update(supplier);
    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật nhà cung cấp thành công";
    //    return supplier;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Supplier.Get(u => u.Id == id && !u.isDeleted);
    //    if (data == null)
    //    {
    //      strMessage = "Nhà cung cấp không tồn tại";
    //      return false;
    //    }
    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Supplier.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa nhà cung cấp thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}
    #endregion

    // thực hiện test SupplierService

    //fake Data Supplier
    public void CreateDataSupplier()
    {
      // Arrange
      for (int i = 1; i < 30; i++)
      {
        _context.Suppliers.Add(new Supplier
        {
          SupplierName = $"SupplierName {i}",
          TaxCode = $"TaxCode {i}",
          Address = $"Address {i}",
          PhoneNumber = "0987316531",
          Email = $"Email {i}",
          BankAccount = $"BankAccount {i}",
          BankName = $"BankName {i}",
          Notes = $"Notes {i}",
          isDeleted = false
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    ///  Test case thực hiện lấy danh sách nhà cung cấp 10 bản ghi
    ///  </summary>
    [Fact]
    public void GetAllSupplierSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.GetAll("", 10, 1, "asc", "SupplierName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách nhà cung cấp khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllSupplierFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.GetAll("", 10, 10, "asc", "SupplierName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Test case thực hiện lấy nhà cung cấp theo id
    /// </summary>
    [Fact]
    public void FindByIdSupplierSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("SupplierName 1", result.SupplierName);
    }

    /// <summary>
    /// Test case thực hiện lấy nhà cung cấp theo id khi không tồn tại
    /// </summary>
    [Fact]
    public void FindByIdSupplierFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Nhà cung cấp không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới nhà cung cấp thành công
    /// </summary>
    [Fact]
    public void CreateSupplierSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Create(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        isDeleted = false
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Tạo mới nhà cung cấp thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới nhà cung cấp thất bại khi trùng tên nhà cung cấp
    /// </summary>
    [Fact]
    public void CreateSupplierNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Create(new Supplier
      {
        SupplierName = "SupplierName 1",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Tên nhà cung cấp đã tồn tại", strMessage);
    }

    /// <summary>
    ///  Test case thực hiện tạo mới nhà cung cấp thất bại khi trùng mã số thuế
    ///  </summary>
    [Fact]
    public void CreateSupplierTaxCodeFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Create(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 1",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Mã số thuế đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới nhà cung cấp khi trùng số điện thoại
    /// </summary>
    [Fact]
    public void CreateSupplierPhoneNumberFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Create(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "0987316531",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới khi trùng email
    /// </summary>
    [Fact]
    public void CreateSupplierEmailFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Create(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 1",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Email nhà cung cấp đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật nhà cung cấp thành công
    /// </summary>
    [Fact]
    public void UpdateSupplierSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Update(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        Id = 6,
        isDeleted = false
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Cập nhật nhà cung cấp thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật nhà cung cấp thất bại khi trùng tên nhà cung cấp
    /// </summary>
    [Fact]
    public void UpdateSupplierNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Update(new Supplier
      {
        SupplierName = "SupplierName 1",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        Id = 6,
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Tên nhà cung cấp đã tồn tại", strMessage);
    }

    /// <summary>
    ///  Test case thực hiện cập nhật nhà cung cấp thất bại khi trùng mã số thuế
    ///  </summary>
    [Fact]
    public void UpdateSupplierTaxCodeFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Update(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 1",
        Address = "Address 30",
        PhoneNumber = "09873161111",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        Id = 6,
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Mã số thuế đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật nhà cung cấp khi trùng số điện thoại
    /// </summary>
    [Fact]
    public void UpdateSupplierPhoneNumberFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _supplierService.Update(new Supplier
      {
        SupplierName = "SupplierName 30",
        TaxCode = "TaxCode 30",
        Address = "Address 30",
        PhoneNumber = "0987316531",
        Email = "Email 30",
        BankAccount = "BankAccount 30",
        BankName = "BankName 30",
        Notes = "Notes 30",
        Id = 6,
        isDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa nhà cung cấp thành công
    /// </summary>
    [Fact]
    public void DeleteSupplierSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.Delete(2, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa nhà cung cấp thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa nhà cung cấp thất bại
    /// </summary>
    [Fact]
    public void DeleteSupplierFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _supplierService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Nhà cung cấp không tồn tại", strMessage);
    }


    #region Trademark Service

    //public class Trademark : MasterEntity
    //{
    //  [Required]
    //  [StringLength(255)]
    //  public string TrademarkName { get; set; } = string.Empty;
    //}

    //public Helpers.PagedResult<Trademark> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.Trademarks.OrderByDescending(u => u.ModifiedDate).AsQueryable();
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.TrademarkName.Contains(name));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Trademark>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Trademark FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Trademark.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      strMessage = "Thương hiệu không tồn tại";
    //      return null;
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Trademark Create(Trademark trademark, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {

    //    var checkPhone = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim());
    //    if (checkPhone != null)
    //    {
    //      strMessage = "Tên thương hiệu đã tồn tại";
    //      return null;
    //    }
    //    trademark.CreateBy = "Admin";
    //    trademark.ModifiedBy = "Admin";
    //    _unitOfWork.Trademark.Add(trademark);
    //    _unitOfWork.Save();
    //    strMessage = "Tạo mới thương hiệu thành công";
    //    return trademark;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Trademark Update(Trademark trademark, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // lấy thông tin thương hiệu
    //    var data = _unitOfWork.Trademark.Get(u => u.Id == trademark.Id, tracked: true);
    //    if (data == null)
    //    {
    //      strMessage = "Thương hiệu không tồn tại";
    //      return null;
    //    }
    //    // kiểm tra số điện thoại thương hiệu đã tồn tại chưa
    //    var trademarkName = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim(), tracked: true);
    //    if (trademarkName != null && trademarkName.Id != trademark.Id)
    //    {
    //      strMessage = "Tên thương hiệu đã tồn tại";
    //      return null;
    //    }
    //    // kiêm tra mã số thueé
    //    data.TrademarkName = trademark.TrademarkName;
    //    data.ModifiedDate = DateTime.Now;
    //    data.ModifiedBy = "Admin";
    //    trademark.ModifiedBy = "Admin";
    //    _unitOfWork.Trademark.Update(trademark);
    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật thành công";
    //    return trademark;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Trademark.Get(u => u.Id == id, tracked: true);
    //    if (data == null)
    //    {
    //      strMessage = "thương hiệu không tồn tại";
    //      return false;
    //    }
    //    _unitOfWork.Trademark.Remove(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}

    #endregion

    // thực hiện test TrademarkService

    //fake Data Trademark
    public void CreateDataTrademark()
    {
      // Arrange
      for (int i = 1; i < 30; i++)
      {
        _context.Trademarks.Add(new Trademark
        {
          TrademarkName = $"TrademarkName {i}",
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách thương hiệu 10 bản ghi
    /// </summary>
    [Fact]
    public void GetAllTrademarkSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.GetAll("", 10, 1, "asc", "TrademarkName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách thương hiệu khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllTrademarkFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.GetAll("", 10, 10, "asc", "TrademarkName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Test case thực hiện lấy thương hiệu theo id
    /// </summary>
    [Fact]
    public void FindByIdTrademarkSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("TrademarkName 1", result.TrademarkName);
    }

    /// <summary>
    /// Test case thực hiện lấy thương hiệu theo id khi không tồn tại
    /// </summary>
    [Fact]
    public void FindByIdTrademarkFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Thương hiệu không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới thương hiệu thành công
    /// </summary>
    [Fact]
    public void CreateTrademarkSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _trademarkService.Create(new Trademark
      {
        TrademarkName = "TrademarkName 30",
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Tạo mới thương hiệu thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới thương hiệu thất bại khi trùng tên thương hiệu
    /// </summary>
    [Fact]
    public void CreateTrademarkNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _trademarkService.Create(new Trademark
      {
        TrademarkName = "TrademarkName 1",
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Tên thương hiệu đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật thương hiệu thành công
    /// </summary>
    [Fact]
    public void UpdateTrademarkSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _trademarkService.Update(new Trademark
      {
        TrademarkName = "TrademarkName 30",
        Id = 6,
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Cập nhật thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật thương hiệu thất bại khi trùng tên thương hiệu
    /// </summary>
    [Fact]
    public void UpdateTrademarkNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _trademarkService.Update(new Trademark
      {
        TrademarkName = "TrademarkName 1",
        Id = 6,
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Tên thương hiệu đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa thương hiệu thành công
    /// </summary>
    [Fact]
    public void DeleteTrademarkSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.Delete(2, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa thương hiệu thất bại
    /// </summary>
    [Fact]
    public void DeleteTrademarkFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _trademarkService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Thương hiệu không tồn tại", strMessage);
    }

    #region Category Service

    //public class Category : MasterEntity
    //{
    //  [Required]
    //  public int ParentID { get; set; }
    //  [Required]
    //  public int CategoryLevel { get; set; }
    //  [Required]
    //  [StringLength(255)]
    //  public string CategoryName { get; set; } = string.Empty;
    //  public string Notes { get; set; } = string.Empty;
    //  public bool IsDeleted { get; set; } = false;
    //}
    //public Helpers.PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    //var query = _db.Categories.AsQueryable().Where(u => u.IsDeleted == false);
    //    var query = _unitOfWork.Category.GetAll(u => !u.IsDeleted).AsQueryable();
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.CategoryName.Contains(name));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = "Không tồn tại bản ghi nào";
    //    return null;
    //  }
    //}
    //public Category FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Category.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      strMessage = "Danh mục sản phẩm không tồn tại";
    //      return null;
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}
    //public Category Create(Category category, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName);
    //    if (checkCategory != null)
    //    {
    //      strMessage = "Danh mục sản phẩm đã tồn tại";
    //      return null;
    //    }

    //    category.CreateBy = "Admin";
    //    category.ModifiedBy = "Admin";
    //    _unitOfWork.Category.Add(category);
    //    _unitOfWork.Save();
    //    strMessage = "Tạo mới danh mục thành công";
    //    return category;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}
    //public Category Update(Category category, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // lấy thông tin danh mục sản phẩm
    //    var data = _unitOfWork.Category.Get(u => u.Id == category.Id, tracked: true);
    //    if (data == null)
    //    {
    //      strMessage = "Danh mục sản phẩm không tồn tại";
    //      return null;
    //    }
    //    if (data.CategoryName != category.CategoryName)
    //    {
    //      // Nếu tên đã thay đổi, kiểm tra trùng lặp với các danh mục khác
    //      var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName && u.Id != category.Id, tracked: true);
    //      if (checkCategory != null)
    //      {
    //        strMessage = "Tên danh mục sản phẩm đã tồn tại";
    //        return null;
    //      }
    //    }
    //    data.CategoryName = category.CategoryName;
    //    data.CategoryLevel = category.CategoryLevel;
    //    data.ParentID = category.ParentID;
    //    data.Notes = category.Notes;
    //    data.ModifiedDate = DateTime.Now;
    //    data.ModifiedBy = category.ModifiedBy;
    //    category.ModifiedBy = "Admin";
    //    _unitOfWork.Category.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật danh mục sản phẩm thành công";
    //    return category;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}
    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Category.Get(u => u.Id == id && !u.IsDeleted, tracked: true);
    //    if (data == null)
    //    {
    //      strMessage = "Danh mục sản phẩm không tồn tại";
    //      return false;
    //    }
    //    data.IsDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Category.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa Danh mục sản phẩm thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}

    #endregion

    // thực hiện test CategoryService

    // fake Data Category
    public void CreateDataCategory()
    {
      // Arrange
      for (int i = 1; i < 30; i++)
      {
        _context.Categories.Add(new Category
        {
          CategoryName = $"CategoryName {i}",
          ParentID = 0,
          CategoryLevel = 1,
          Notes = $"Notes {i}",
          IsDeleted = false
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách danh mục sản phẩm 10 bản ghi
    /// </summary>
    [Fact]
    public void GetAllCategorySuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.GetAll("", 10, 1, "asc", "CategoryName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách danh mục sản phẩm khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllCategoryFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.GetAll("", 10, 10, "asc", "CategoryName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Test case thực hiện lấy danh mục sản phẩm theo id
    /// </summary>
    [Fact]
    public void FindByIdCategorySuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("CategoryName 1", result.CategoryName);
    }

    /// <summary>
    /// Test case thực hiện lấy danh mục sản phẩm theo id khi không tồn tại
    /// </summary>
    [Fact]
    public void FindByIdCategoryFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.FindById(40, out strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới danh mục sản phẩm thành công
    /// </summary>
    [Fact]
    public void CreateCategorySuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _categoryProductService.Create(new Category
      {
        CategoryName = "CategoryName 30",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes 30",
        IsDeleted = false
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Tạo mới danh mục thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới danh mục sản phẩm thất bại khi trùng tên danh mục
    /// </summary>
    [Fact]
    public void CreateCategoryNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _categoryProductService.Create(new Category
      {
        CategoryName = "CategoryName 1",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes 30",
        IsDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Danh mục sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật danh mục sản phẩm thành công
    /// </summary>
    [Fact]
    public void UpdateCategorySuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _categoryProductService.Update(new Category
      {
        CategoryName = "CategoryName 30",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes 30",
        Id = 6,
        IsDeleted = false
      }, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Cập nhật danh mục sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật danh mục sản phẩm thất bại khi trùng tên danh mục
    /// </summary>
    [Fact]
    public void UpdateCategoryNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _categoryProductService.Update(new Category
      {
        CategoryName = "CategoryName 1",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes 30",
        Id = 6,
        IsDeleted = false
      }, out strMessage);
      Assert.Null(result);
      Assert.Equal("Tên danh mục sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa danh mục sản phẩm thành công
    /// </summary>
    [Fact]
    public void DeleteCategorySuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.Delete(2, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa Danh mục sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa danh mục sản phẩm thất bại
    /// </summary>
    [Fact]
    public void DeleteCategoryFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryProductService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Danh mục sản phẩm không tồn tại", strMessage);
    }

    #region Product Service

    //public class ProductDTO
    //{
    //  public Product Product { get; set; }
    //  public IEnumerable<ProductImages> ProductImages { get; set; }
    //  public Category CategoryProduct { get; set; }
    //}


    //public class Product : MasterEntity
    //{
    //  public int TrademarkId { get; set; }
    //  [ForeignKey("TrademarkId")]
    //  [ValidateNever]
    //  public Trademark? Trademark { get; set; }
    //  [Required]
    //  public int CategoryId { get; set; }
    //  [ForeignKey("CategoryId")]
    //  [ValidateNever]
    //  public Category? Category { get; set; }
    //  [Required]
    //  [StringLength(255)]

    //  public string Color { get; set; } = string.Empty;
    //  public string ProductName { get; set; } = string.Empty;
    //  [Required]
    //  public string Description { get; set; } = string.Empty;
    //  [Required]
    //  public decimal Price { get; set; } = 0;
    //  public int? PriorityLevel { get; set; } = 0;
    //  public double Quantity { get; set; } = 0;
    //  public string? Notes { get; set; } = string.Empty;
    //  public bool isActive { get; set; } = false;
    //  public bool isDeleted { get; set; } = false;

    //}

    //public class ProductImages : MasterEntity
    //{
    //  public int ProductId { get; set; }
    //  [ForeignKey("ProductId")]
    //  [ValidateNever]
    //  public Product? Product { get; set; }
    //  public string ImageName { get; set; } = string.Empty;
    //  public string ImagePath { get; set; } = string.Empty;
    //  public bool IsDefault { get; set; }
    //  public bool IsActive { get; set; }
    //  public bool IsDeleted { get; set; }


    //  public string DisplayValue
    //  {
    //    get
    //    {
    //      return string.Format("{0} - {1}", Id, ImageName);
    //    }
    //  }
    //}

    //public Helpers.PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _unitOfWork.Product.GetAll(u => u.isDeleted == false, "Category,Trademark").AsQueryable();

    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.ProductName.Contains(name));
    //    }

    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);

    //    // Project query results to ProductDTO
    //    var pagedResult = Helpers.PagedResult<ProductDTO>.CreatePagedResult(query.Select(p => new ProductDTO
    //    { Product = p, ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == p.Id, null), CategoryProduct = p.Category }), pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public ProductDTO FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    ProductDTO productDTO = new ProductDTO
    //    {
    //      Product = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted == false),
    //      ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == id).ToList(),
    //      CategoryProduct = _unitOfWork.Category.Get(u => u.Id == id)
    //    };
    //    if (productDTO == null)
    //    {
    //      strMessage = "Sản phẩm không tồn tại";
    //      return null;
    //    }
    //    else
    //    {
    //      return productDTO;
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public Product Create(Product product, List<IFormFile>? files, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  //_unitOfWork.BeginTransaction(); // Bắt đầu giao dịch

    //  try
    //  {
    //    if (product.Id == 0)
    //    {
    //      var checkProduct = _unitOfWork.Product.Get(x => x.ProductName == product.ProductName);
    //      if (checkProduct != null)
    //      {
    //        strMessage = "Sản phẩm đã tồn tại";
    //        return null;
    //      }

    //      product.CreateBy = "Admin";
    //      product.ModifiedBy = "Admin";

    //      _unitOfWork.Product.Add(product);
    //      _unitOfWork.Save();

    //      if (files != null && files.Count > 0)
    //      {
    //        foreach (var file in files)
    //        {
    //          var image = new ProductImages();
    //          image.ProductId = product.Id;
    //          image.ImageName = file.FileName;
    //          //image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
    //          image.IsDefault = false;
    //          image.IsActive = true;
    //          image.IsDeleted = false;
    //          image.CreateBy = "Admin";
    //          image.ModifiedBy = "Admin";
    //          _unitOfWork.ProductImages.Add(image);
    //        }
    //        _unitOfWork.Save();
    //      }

    //      strMessage = "Tạo mới thành công";
    //      _unitOfWork.Commit(); // Commit giao dịch
    //      return product;
    //    }
    //    // thực hiện update product
    //    var model = _unitOfWork.Product.Get(x => x.Id == product.Id);
    //    model.ProductName = product.ProductName;
    //    model.Price = product.Price;
    //    model.PriorityLevel = product.PriorityLevel;
    //    model.Description = product.Description;
    //    model.Description = product.Description;
    //    model.CategoryId = product.CategoryId;
    //    model.TrademarkId = product.TrademarkId;
    //    model.isActive = product.isActive;
    //    model.isDeleted = product.isDeleted;
    //    model.ModifiedBy = "Admin";
    //    model.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Product.Update(model);
    //    _unitOfWork.Save();
    //    if (files.Count > 0)
    //    {
    //      var productImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == product.Id);

    //      foreach (var item in productImages)
    //      {
    //        //ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, item.ImagePath);
    //        //_unitOfWork.ProductImages.Remove(item);
    //      }
    //    }

    //    if (files != null)
    //    {
    //      foreach (var file in files)
    //      {
    //        if (!ImageHelper.CheckImage(file))
    //        {
    //          strMessage = "File không đúng định dạng hoặc dung lượng lớn hơn 10MB";
    //          _unitOfWork.Rollback();
    //          return null;
    //        }

    //        var image = new ProductImages();
    //        image.ProductId = product.Id;
    //        image.ImageName = file.FileName;
    //        //image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
    //        image.IsDefault = false;
    //        image.IsActive = true;
    //        image.IsDeleted = false;
    //        image.CreateBy = "Admin";
    //        image.ModifiedBy = "Admin";
    //        _unitOfWork.ProductImages.Add(image);
    //      }
    //    }

    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật sản phẩm thành công";
    //    _unitOfWork.Commit();
    //    return product;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    _unitOfWork.Rollback();
    //    return null;
    //  }
    //}

    //public Product Update(Product model, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // kiêm tra mã số thueé
    //    model.ModifiedBy = "Admin";
    //    _unitOfWork.Product.Update(model);
    //    _unitOfWork.Save();
    //    strMessage = "Cập nhật thành công";
    //    return model;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted);
    //    if (data == null)
    //    {
    //      strMessage = "Sản phẩm không tồn tại";
    //      return false;
    //    }

    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Product.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = "Xóa sản phẩm thành công";
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return false;
    //  }
    //}

    //public bool DeleteProductImage(int productId, string imageName, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var productImage = _unitOfWork.ProductImages.Get(x => x.ProductId == productId && x.ImageName == imageName);
    //    if (productImage == null)
    //    {
    //      strMessage = "Hình ảnh sản phẩm không tồn tại!";
    //      return false;
    //    }

    //    if (!ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, productImage.ImagePath))
    //    {
    //      strMessage = "Xóa ảnh không thành công!";
    //      return false;
    //    }

    //    _unitOfWork.ProductImages.Remove(productImage);
    //    _unitOfWork.Save();
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = "Có lỗi xảy ra";
    //    return false;
    //  }

    //  throw new NotImplementedException();
    //}

    #endregion

    // dừng  test ProductService
    //fake Data Product

    public void CreateDataProduct()
    {
      // Arrange
      for (int i = 1; i < 30; i++)
      {
        _context.Products.Add(new Product
        {
          TrademarkId = i,
          CategoryId = i,
          Color = $"Color {i}",
          ProductName = $"ProductName {i}",
          Description = $"Description {i}",
          Price = 1000,
          PriorityLevel = 1,
          Quantity = 10,
          Notes = $"Notes {i}",
          isActive = true,
          isDeleted = false
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case thực hiện lấy danh sách sản phẩm 10 bản ghi
    /// </summary>
    [Fact]
    public void GetAllProductSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.GetAll("", 10, 1, "asc", "ProductName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }


    /// <summary>
    /// Test case thực hiện lấy danh sách sản phẩm khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllProductFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.GetAll("", 10, 10, "asc", "ProductName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Tese case thực hiện lấy sản phẩm theo id
    /// </summary>
    [Fact]
    public void GetProductSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("ProductName 1", result.Product.ProductName);
    }

    /// <summary>
    /// Test case thực hiện lấy sản phẩm theo id khi không tồn tại
    /// </summary>
    [Fact]
    public void GetProductFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Sản phẩm không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới sản phẩm thành công
    /// </summary>
    [Fact]
    public void CreateProductSuccess()
    {
      string strMessage = string.Empty;

      // Act
      var result = _productService.Create(new Product
      {
        TrademarkId = 1,
        CategoryId = 1,
        Color = "Color 30",
        ProductName = "ProductName 30",
        Description = "Description 30",
        Price = 1000,
        PriorityLevel = 1,
        Quantity = 10,
        Notes = "Notes 30",
        isActive = true,
        isDeleted = false
      }, null, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Tạo mới thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện tạo mới sản phẩm thất bại khi trùng tên sản phẩm
    /// </summary>
    [Fact]
    public void CreateProductNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _productService.Create(new Product
      {
        TrademarkId = 1,
        CategoryId = 1,
        Color = "Color 1",
        ProductName = "ProductName 1",
        Description = "Description 30",
        Price = 1000,
        PriorityLevel = 1,
        Quantity = 10,
        Notes = "Notes 30",
        isActive = true,
        isDeleted = false
      }, null, out strMessage);
      Assert.Null(result);
      Assert.Equal("Sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật sản phẩm thành công
    /// </summary>
    [Fact]
    public void UpdateProductSuccess()
    {
      string strMessage = string.Empty;
      // Tạo danh sách đối tượng IFormFile
      List<IFormFile> files = new List<IFormFile>();

      // Tạo một đối tượng IFormFile và thêm vào danh sách
      IFormFile file = new FormFile(
          new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")),
          0,
          4000,
          "Data",
          "dummy.jpg")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/jpeg"
      };
      files.Add(file);

      // Act
      var result = _productService.Create(new Product
      {
        TrademarkId = 1,
        CategoryId = 1,
        Color = "Color 30",
        ProductName = "ProductName 30",
        Description = "Description 30",
        Price = 1000,
        PriorityLevel = 1,
        Quantity = 10,
        Notes = "Notes 30",
        isActive = true,
        isDeleted = false,
        Id = 6
      }, files, out strMessage);

      Assert.NotNull(result);
      Assert.Equal("Cập nhật sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật sản phẩm thất bại khi trùng tên sản phẩm
    /// </summary>
    [Fact]
    public void UpdateProductNameFail()
    {
      string strMessage = string.Empty;

      // Act
      var result = _productService.Create(new Product
      {
        TrademarkId = 1,
        CategoryId = 1,
        Color = "Color 1",
        ProductName = "ProductName 1",
        Description = "Description 30",
        Price = 1000,
        PriorityLevel = 1,
        Quantity = 10,
        Notes = "Notes 30",
        isActive = true,
        isDeleted = false,
        Id = 6
      }, null, out strMessage);
      Assert.Null(result);
      Assert.Equal("Sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật sản phẩm khi file ảnh không đúng định dạng
    /// </summary>
    [Fact]
    public void UpdateProductImageFail()
    {
      string strMessage = string.Empty;
      // Tạo danh sách đối tượng IFormFile
      List<IFormFile> files = new List<IFormFile>();

      // Tạo một đối tượng IFormFile và thêm vào danh sách
      IFormFile file = new FormFile(
                 new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")),
                          0,
                                   4000,
                                            "Data",
                                                     "dummy.pdf")
      {
        Headers = new HeaderDictionary(),
        ContentType = "application/pdf"
      };
      files.Add(file);

      // Act
      var result = _productService.Create(new Product
      {
        TrademarkId = 1,
        CategoryId = 1,
        Color = "Color 30",
        ProductName = "ProductName 30",
        Description = "Description 30",
        Price = 1000,
        PriorityLevel = 1,
        Quantity = 10,
        Notes = "Notes 30",
        isActive = true,
        isDeleted = false,
        Id = 6
      }, files, out strMessage);
      Assert.Null(result);
      Assert.Equal("File không đúng định dạng hoặc dung lượng lớn hơn 10MB", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa sản phẩm thành công
    /// </summary>
    [Fact]
    public void DeleteProductSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.Delete(2, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện xóa sản phẩm thất bại
    /// </summary>
    [Fact]
    public void DeleteProductFaild()
    {
      string strMessage = string.Empty;
      // Act
      var result = _productService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Sản phẩm không tồn tại", strMessage);
    }

  }
}
