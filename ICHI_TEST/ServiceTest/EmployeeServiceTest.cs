using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Service;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_TEST.ServiceTest
{
  public class EmployeeServiceTest
  {
    private readonly PcsApiContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employeeService;

    public EmployeeServiceTest()
    {
      _context = ContextGenerator.GeneratorDb();
      _unitOfWork = new UnitOfWork(_context);
      _employeeService = new EmployeeService(_unitOfWork, _context);
    }


    [Fact]
    public void Test()
    {
      string str = "Test";
      var result = _employeeService.FindById(1, out str);

      Assert.NotNull(result);
    }

    /// <summary>
    /// Test case cập nhật nhân viên thành công
    /// </summary>
    [Fact]
    public void UpdateEmployeeSuccess()
    {
      string strMessage = string.Empty;

      // Tạo giả lập IFormFile
      var fileMock = new Mock<IFormFile>();
      // Thiết lập các thuộc tính của giả lập
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
      fileMock.Setup(f => f.Length).Returns(1234);

      Random random = new Random();
      int randomNumber = random.Next(1, 9);
      // Act
      var result = _employeeService.Update(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Id = 1,
        Birthday = DateTime.Now,
        PhoneNumber = $"098731691{randomNumber}",
        Email = "nhanvien@gmail.com",
        FullName = "FullName 30",
        isDeleted = false,
        UserId = "demo@gmail.com"
      }, fileMock.Object, out strMessage);
      Assert.NotNull(result);
      Assert.Equal("Cập nhật nhân viên thành công", strMessage);
    }

    /// <summary>
    /// Test case cập nhật nhân viên không tồn tại
    /// </summary>
    [Fact]
    public void UpdateEmployeeNotExist()
    {
      string strMessage = string.Empty;

      // Tạo giả lập IFormFile
      var fileMock = new Mock<IFormFile>();
      // Thiết lập các thuộc tính của giả lập
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
      fileMock.Setup(f => f.Length).Returns(1234);

      // Act
      var result = _employeeService.Update(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Id = 100,
        Birthday = DateTime.Now,
        PhoneNumber = "0987316983",
        Email = ""
      }, fileMock.Object, out strMessage);
      Assert.Null(result);
      Assert.Equal("Nhân viên không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case cập nhật nhân viên số điện thoại đã tồn tại
    /// </summary>
    [Fact]
    public void UpdateEmployeePhoneExist()
    {
      string strMessage = string.Empty;

      // Tạo giả lập IFormFile
      var fileMock = new Mock<IFormFile>();
      // Thiết lập các thuộc tính của giả lập
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
      fileMock.Setup(f => f.Length).Returns(1234);

      // Act
      var result = _employeeService.Update(new Employee
      {
        Address = "Address 30",
        Avatar = "Avatar",
        Id = 2,
        Birthday = DateTime.Now,
        PhoneNumber = "0346790421",
        Email = ""
      }, fileMock.Object, out strMessage);
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }

    ///// <summary>
    ///// Test case thực hiện xóa nhân viên thành công
    ///// </summary>
    //[Fact]
    //public void DeleteEmployeeSuccess()
    //{
    //  string strMessage = string.Empty;
    //  // Act
    //  var result = _employeeService.Delete(2, out strMessage);
    //  // Assert
    //  Assert.True(result);
    //  Assert.Equal("Xóa nhân viên thành công", strMessage);
    //}

    ///// <summary>
    ///// Test case thực hiện xóa nhân viên thất bại
    ///// </summary>
    //[Fact]
    //public void DeleteEmployeeFaild()
    //{
    //  string strMessage = string.Empty;
    //  // Act
    //  var result = _employeeService.Delete(50, out strMessage);
    //  // Assert
    //  Assert.False(result);
    //  Assert.Equal("Nhân viên không tồn tại", strMessage);
    //}




  }

}
