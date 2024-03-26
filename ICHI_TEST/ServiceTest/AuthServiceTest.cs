using API.Model;
using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_TEST.ServiceTest
{
  public class AuthServiceTest
  {
    private readonly PcsApiContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userSerivce;
    private readonly IAuthService _authService;

    public AuthServiceTest()
    {
      _context = ContextGenerator.Generator();
      _unitOfWork = new UnitOfWork(_context);
      _authService = new AuthService(_unitOfWork);
      _userSerivce = new UserService(_unitOfWork, _context);
      CreateRole();
      CreateDataResgister();
    }
    #region Auth service

    //public class UserRegister
    //{
    //  [StringLength(64)]
    //  [Required]
    //  public string Password { get; set; } = string.Empty;

    //  [Required]
    //  [StringLength(50)]
    //  public string Email { get; set; } = string.Empty;

    //  [StringLength(50)]
    //  public string FullName { get; set; } = string.Empty;

    //  [StringLength(20)]
    //  public string PhoneNumber { get; set; } = string.Empty;

    //  public string? Role { get; set; } = string.Empty;

    //  public string Gender { get; set; } = string.Empty;

    //  public DateTime Birthday { get; set; }


    //}


    //public class UserLogin
    //{
    //  public string UserName { get; set; }
    //  public string Password { get; set; }
    //}


    //public class UserChangePassword
    //{
    //  public string UserName { get; set; }

    //  public string? oldPassword { get; set; }
    //  public string Password { get; set; }
    //  public string NewPassword { get; set; }
    //}
    //public string Register(UserRegister userRegister, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    using (var transaction = _db.Database.BeginTransaction())
    //    {
    //      // kiểm tra người dùng có hợp lệ không
    //      if (ExistsByPhoneNumber(userRegister.PhoneNumber.Trim()))
    //      {
    //        strMessage = "Số điện thoại đã tồn tại";
    //        return null;
    //      }
    //      if (ExistsByUserNameOrEmail(userRegister.Email.Trim()) != null)
    //      {
    //        strMessage = "User đã tồn tại";
    //        return null;
    //      }
    //      // mật khẩu phải > 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt

    //      if (!userRegister.Password.Any(char.IsUpper) || !userRegister.Password.Any(char.IsLower) || !userRegister.Password.Any(char.IsDigit) || userRegister.Password.Length < 8)
    //      {
    //        strMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt";
    //        return null;
    //      }
    //      //userRegister.Role= AppSettings.EMPLOYEE;
    //      User user = new User();
    //      MapperHelper.Map<UserRegister, User>(userRegister, user);
    //      string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password.Trim(), salt);
    //      user.Password = hashedPassword;
    //      user.IsLocked = false;
    //      user.Avatar = AppSettings.AvatarDefault;
    //      user.CreateBy = userRegister.Email;
    //      user.ModifiedBy = userRegister.Email;
    //      user.Email = userRegister.Email.Trim();
    //      _unitOfWork.User.Add(user);
    //      _unitOfWork.Save();
    //      if (userRegister.Role == AppSettings.EMPLOYEE)
    //      {
    //        // insert vào employee
    //        Employee employee = new Employee()
    //        {
    //          PhoneNumber = userRegister.PhoneNumber,
    //          FullName = userRegister.FullName,
    //          UserId = user.Email,
    //          Gender = userRegister.Gender,
    //          Birthday = userRegister.Birthday,

    //        };
    //        _unitOfWork.Employee.Add(employee);
    //        _unitOfWork.Save();
    //        UserRole userRole = new UserRole()
    //        {
    //          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.EMPLOYEE).Id,
    //          UserId = user.Email,
    //        };
    //        _unitOfWork.UserRole.Add(userRole);
    //        _unitOfWork.Save();
    //      }
    //      else
    //      {
    //        // insert vào customer
    //        Customer customer = new Customer()
    //        {
    //          PhoneNumber = userRegister.PhoneNumber,
    //          FullName = userRegister.FullName,
    //          UserId = user.Email,
    //          Gender = userRegister.Gender,
    //          Birthday = userRegister.Birthday,
    //        };
    //        _unitOfWork.Customer.Add(customer);
    //        _unitOfWork.Save();
    //        UserRole userRole = new UserRole()
    //        {
    //          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.USER).Id,
    //          UserId = user.Email,
    //        };
    //        _unitOfWork.UserRole.Add(userRole);
    //        _unitOfWork.Save();
    //      }
    //      transaction.Commit();
    //      strMessage = "Đăng ký thành công";
    //      var accessToken = GenerateAccessToken(user);
    //      SetJWTCookie(accessToken);
    //      return accessToken;
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = "Có lỗi xảy ra";
    //    return null;
    //  }

    //}

    //public string Login(UserLogin userLogin, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByUserNameOrEmail(userLogin.UserName);
    //    if (loginUser == null)
    //    {
    //      strMessage = "Tài khoản không tồn tại";
    //      return null;
    //    }
    //    // nếu loginuser tồn tại thì kiểm tra mật khẩu
    //    if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loginUser.Password))
    //    {
    //      // nếu người dùng đăng nhập sai thì thực hiện cập nhật số lần đăng nhập sai
    //      loginUser.FailedPassAttemptCount++;
    //      // nếu đăng nhập sai quá 5 lần thì khóa tài khoản
    //      if (loginUser.FailedPassAttemptCount >= 5)
    //      {
    //        loginUser.IsLocked = true;
    //        strMessage = "Tài khoản đã bị khóa";
    //      }
    //      else
    //      {
    //        strMessage = "Mật khẩu không đúng";
    //      }
    //      _unitOfWork.User.Update(loginUser);
    //      _unitOfWork.Save();
    //      return null;
    //    }
    //    strMessage = "Đăng nhập thành công";
    //    var accessToken = GenerateAccessToken(loginUser);
    //    SetJWTCookie(accessToken);
    //    return accessToken;
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}
    //public string ChangePassword(UserChangePassword user, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var userExist = ExistsByUserNameOrEmail(user.UserName);
    //    if (userExist != null && BCrypt.Net.BCrypt.Verify(user.oldPassword, userExist.Password))
    //    {
    //      string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
    //      userExist.Password = hashedPassword;
    //      userExist.ModifiedBy = user.UserName;
    //      userExist.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(userExist);
    //      _unitOfWork.Save();
    //      return "Đổi mật khẩu thành công";
    //    }
    //    else
    //    {
    //      strMessage = "Mật khẩu cũ không đúng";
    //      return null;
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }
    //}
    //public string ForgotPassword(string email, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByUserNameOrEmail(email);
    //    if (loginUser == null)
    //    {
    //      strMessage = "Quên mật khẩu thất bại";
    //      return null;
    //    }
    //    var emailService = new EmailService(_configuration);
    //    // random mật khẩu mới
    //    Random random = new Random();
    //    string randomString = new string(Enumerable.Repeat(AppSettings.Encode, random.Next(8, 16))
    //               .Select(s => s[random.Next(s.Length)]).ToArray());
    //    string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomString, salt);
    //    User user = _unitOfWork.User.Get(u => u.Email == loginUser.Email || u.Email.Equals(email));
    //    user.Password = hashedPassword;
    //    user.ModifiedBy = loginUser.Email;
    //    user.ModifiedDate = DateTime.Now;
    //    _unitOfWork.User.Update(user);
    //    _unitOfWork.Save();
    //    string url = "Mật khẩu mới của bạn là: " + randomString;
    //    string body = "Click vào link sau để đổi mật khẩu: " + url;
    //    emailService.SendEmail(email, "Reset password", url);
    //    strMessage = "Gửi email thành công";
    //    return "Gửi email thành công";
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    strMessage = ex.ToString();
    //    return null;
    //  }

    //}
    #endregion

    // thực hiện test AuthService

    // tạo data fake role

    public void CreateRole()
    {
      List<Role> roles = new List<Role>();
      roles.Add(new Role
      {
        RoleName = AppSettings.ADMIN,
        Description = "ADMIN",
        CreateBy = "ADMIN",
        CreateDate = DateTime.Now
      });
      roles.Add(new Role
      {
        RoleName = AppSettings.EMPLOYEE,
        Description = "EMPLOYEE",
        CreateBy = "ADMIN",
        CreateDate = DateTime.Now
      });
      roles.Add(new Role
      {
        RoleName = AppSettings.USER,
        Description = "USER",
        CreateBy = "ADMIN",
        CreateDate = DateTime.Now
      });
      _context.Roles.AddRangeAsync(roles);
      _context.SaveChanges();
    }

    // data fake user
    public void CreateDataResgister()
    {
      string strMessage = string.Empty;

      for (int i = 1; i <= 5; i++)
      {
        var users = new UserRegister
        {
          Birthday = DateTime.Now,
          Email = $"Email 123{i}",
          FullName = "FullName",
          Gender = "Nam",
          PhoneNumber = $"03467904{i}",
          Password = "Tien1234@",
          Role = "USER",
        };
        _authService.Register(users, out strMessage);
      }

      var user = new UserRegister
      {
        Birthday = DateTime.Now,
        Email = "tien01nx@gmail.com",
        FullName = "FullName",
        Gender = "Nam",
        PhoneNumber = $"0346790412",
        Password = "Tien1234@",
        Role = "USER",
      };
      _authService.Register(user, out strMessage);
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case kiểm tra đăng ký thành công
    /// </summary>
    [Fact]
    public void RegisterSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Register(new UserRegister
      {
        Birthday = DateTime.Now,
        Email = "tien123@gmail.com",
        FullName = "FullName",
        Gender = "Nam",
        PhoneNumber = "0346790482",
        Password = "Tien1234@",
        Role = "USER",
      }, out strMessage);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Đăng ký thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện đăng ký thất bại khi trùng số điện thoại
    /// </summary>
    [Fact]
    public void RegisterPhoneNumberFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Register(new UserRegister
      {
        Birthday = DateTime.Now,
        Email = "Email 83763",
        FullName = "FullName",
        Gender = "Nam",
        PhoneNumber = "034679044",
        Password = "Tien1234@",
        Role = "USER",
      }, out strMessage);

      // Assert
      Assert.Null(result);
      Assert.Equal("Số điện thoại đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện đăng ký thất bại khi trùng email
    /// </summary>
    [Fact]
    public void RegisterEmailFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Register(new UserRegister
      {
        Birthday = DateTime.Now,
        Email = "Email 1232",
        FullName = "FullName",
        Gender = "Nam",
        PhoneNumber = "03467904444",
        Password = "Tien1234@",
        Role = "USER",

      }, out strMessage);

      // Assert
      Assert.Null(result);
      Assert.Equal("User đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case đăng nhập thành công
    /// </summary>
    [Fact]
    public void LoginSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Login(new UserLogin
      {
        UserName = "Email 1231",
        Password = "Tien1234@",
      }, out strMessage);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Đăng nhập thành công", strMessage);
    }

    /// <summary>
    /// Test case đăng nhập thất bại khi tài khoản không tồn tại
    /// </summary> 
    [Fact]
    public void LoginEmailFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Login(new UserLogin
      {
        UserName = "Email 123",
        Password = "Tien1234555@"
      }, out strMessage);

      // Assert
      Assert.Null(result);
      Assert.Equal("Tài khoản không tồn tại", strMessage);
    }


    /// <summary>
    /// Test case đăng nhập thất bại khi mật khẩu không đúng
    /// </summary>
    [Fact]
    public void LoginPasswordFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.Login(new UserLogin
      {
        UserName = "Email 1233",
        Password = "Tien1234555@"
      }, out strMessage);

      // Assert
      Assert.Null(result);
      Assert.Equal("Mật khẩu không đúng", strMessage);
    }

    ///// <summary>
    ///// test case đăng nhập sai quá 6 lần thì tài khoản bị khóa
    ///// </summary>
    //[Fact]
    //public void LoginLockAccount()
    //{
    //  string strMessage = string.Empty;
    //  // Act
    //  for (int i = 0; i < 6; i++)
    //  {
    //    var result = _authService.Login(new UserLogin
    //    {
    //      UserName = "Email Email 1232",
    //      Password = "Tien1234555@"
    //    }, out strMessage);
    //  }
    //  // Assert
    //  Assert.Equal("Tài khoản đã bị khóa", strMessage);

    //}

    /// <summary>
    /// Test case thực hiện đổi mật khẩu thành công
    /// </summary>
    [Fact]
    public void ChangePasswordSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.ChangePassword(new UserChangePassword
      {
        UserName = "Email 1232",
        oldPassword = "Tien1234@",
        Password = "Tien1234@",
        NewPassword = "Tien1234@123",
      }, out strMessage);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Đổi mật khẩu thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện đổi mật khẩu thất bại khi mật khẩu cũ không đúng
    /// </summary>  
    [Fact]
    public void ChangePasswordOldPasswordFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.ChangePassword(new UserChangePassword
      {
        UserName = "Email 1232",
        oldPassword = "Tien1234@123",
        Password = "Tien1234@",
        NewPassword = "Tien1234@123",
      }, out strMessage);

      // Assert
      Assert.Null(result);
      Assert.Equal("Mật khẩu cũ không đúng", strMessage);
    }

    /// <summary>
    /// Test case thực hiện quên mật khẩu thành công
    /// </summary>
    [Fact]
    public void ForgotPasswordSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.ForgotPassword("tien01nx@gmail.com", out strMessage);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Gửi email thành công", strMessage);
    }

    /// <summary>
    /// Testcase thực hiện quên mật khẩu thất bại khi email không tồn tại
    /// </summary>
    [Fact]
    public void ForgotPasswordEmailFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.ForgotPassword("conga123@gmail.com", out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Quên mật khẩu thất bại", strMessage);
    }

    /// <summary>
    /// Test case khóa tài khoản thành công
    /// </summary>
    [Fact]
    public void LockAccountSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.LockAccount("Email 1232", true, out strMessage);

      // Assert
      Assert.True(result);
      Assert.Equal("Mở khóa tài khoản thành công", strMessage);
    }


    /// <summary>
    /// Test case mở tài khoản thành công
    /// </summary>
    [Fact]
    public void UnLockAccountSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _authService.LockAccount("Email 1232", false, out strMessage);

      // Assert
      Assert.True(result);
      Assert.Equal("Khóa tài khoản thành công", strMessage);
    }


  }
}
