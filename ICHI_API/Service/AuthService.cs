using API.Model;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Extension;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  using static ICHI_API.Helpers.Constants;

  public class AuthService : IAuthService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
      _configuration = configuration;
      _httpContextAccessor = httpContextAccessor;
    }
    public string Login(UserLogin userLogin, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        User loginUser = _unitOfWork.User.Get(u => u.Email.Equals(userLogin.UserName));
        if (loginUser == null)
        {
          throw new BadRequestException(Constants.ACCOUNTNOTFOUNF);
        }
        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loginUser.Password))
        {
          loginUser.FailedPassAttemptCount++;
          if (loginUser.FailedPassAttemptCount >= 5)
          {
            loginUser.IsLocked = true;
            strMessage = Constants.ACCOUNTLOCK;
          }
          else
          {
            strMessage = Constants.PASSWORDOLDPFAIL;
          }
          _unitOfWork.User.Update(loginUser);
          _unitOfWork.Save();
          return strMessage;
        }
        strMessage = Constants.LOGINSUCCESS;
        var accessToken = _unitOfWork.User.GenerateAccessToken(loginUser);
        return accessToken;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string RegisterCustomer(UserRegister userRegister, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        if (_unitOfWork.Customer.ExistsBy(u => u.PhoneNumber == userRegister.PhoneNumber.Trim()))
          throw new BadRequestException(PHONENUMBEREXIST);
        if (_unitOfWork.User.ExistsBy(u => u.Email == userRegister.Email.Trim()))
          throw new BadRequestException(USEREXIST);
        User user = new User();
        MapperHelper.Map<UserRegister, User>(userRegister, user);
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password.Trim(), salt);
        user.Password = hashedPassword;
        user.Avatar = AppSettings.AvatarDefault;
        user.Email = userRegister.Email.Trim();
        _unitOfWork.User.Add(user);
        _unitOfWork.Save();
        Customer customer = new Customer()
        {
          PhoneNumber = userRegister.PhoneNumber,
          FullName = userRegister.FullName,
          UserId = user.Email,
          Gender = userRegister.Gender,
          Birthday = userRegister.Birthday,
        };
        _unitOfWork.Customer.Add(customer);
        _unitOfWork.Save();
        UserRole userRole = new UserRole()
        {
          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.USER).Id,
          UserId = user.Email,
        };
        _unitOfWork.UserRole.Add(userRole);
        _unitOfWork.Save();
        strMessage = Constants.REGISTERSUCCESS;
        var accessToken = _unitOfWork.User.GenerateAccessToken(user);
        _unitOfWork.Commit();
        return accessToken;
      }
      catch (Exception)
      {
        _unitOfWork.Rollback();
        throw;
      }
    }
    public string RegisterEmployee(UserRegister userRegister, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        if (_unitOfWork.Customer.ExistsBy(u => u.PhoneNumber == userRegister.PhoneNumber.Trim()))
          throw new BadRequestException(Constants.PHONENUMBEREXIST);
        if (_unitOfWork.User.ExistsBy(u => u.Email == userRegister.Email.Trim()))
          throw new BadRequestException(Constants.USEREXIST);
        User user = new User();
        MapperHelper.Map<UserRegister, User>(userRegister, user);
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password.Trim(), salt);
        user.Password = hashedPassword;
        user.Avatar = AppSettings.AvatarDefault;
        user.Email = userRegister.Email.Trim();
        _unitOfWork.User.Add(user);
        _unitOfWork.Save();
        Employee employee = new Employee()
        {
          PhoneNumber = userRegister.PhoneNumber,
          FullName = userRegister.FullName,
          UserId = user.Email,
          Gender = userRegister.Gender,
          Birthday = userRegister.Birthday,
          Address = userRegister.Address,
        };
        _unitOfWork.Employee.Add(employee);
        _unitOfWork.Save();
        UserRole userRole = new UserRole()
        {
          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.EMPLOYEE).Id,
          UserId = user.Email,
        };
        _unitOfWork.UserRole.Add(userRole);
        _unitOfWork.Save();
        strMessage = Constants.REGISTERSUCCESS;
        var accessToken = _unitOfWork.User.GenerateAccessToken(user);
        _unitOfWork.Commit();
        return accessToken;
      }
      catch (Exception)
      {
        _unitOfWork.Rollback();
        throw;
      }
    }
    public bool ChangePassword(UserChangePassword user, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var userExist = _unitOfWork.User.Get(u => u.Email.Equals(user.UserName));
        if (userExist != null && BCrypt.Net.BCrypt.Verify(user.oldPassword, userExist.Password))
        {
          string salt = BCrypt.Net.BCrypt.GenerateSalt();
          string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
          userExist.Password = hashedPassword;
          _unitOfWork.User.Update(userExist);
          _unitOfWork.Save();
          strMessage = Constants.PASSWORDSUCCESS;
          return true;
        }
        else
        {
          throw new BadRequestException(Constants.PASSWORDOLDPFAIL);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }
    public bool ForgotPassword(string email, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        if (!_unitOfWork.User.ExistsBy(u => u.Email.Equals(email)))
        {
          throw new BadRequestException(Constants.ACCOUNTNOTFOUNF);
        }
        var emailService = new EmailService(_configuration);
        Random random = new Random();
        string randomString = new string(Enumerable.Repeat(AppSettings.Encode, random.Next(8, 16))
                   .Select(s => s[random.Next(s.Length)]).ToArray());
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomString, salt);
        User user = _unitOfWork.User.Get(u => u.Email == email || u.Email.Equals(email));
        user.Password = hashedPassword;
        user.ModifiedBy = email;
        user.ModifiedDate = DateTime.Now;
        _unitOfWork.User.Update(user);
        _unitOfWork.Save();
        string url = SENDMAILSUCCESS + randomString;
        string body = SENDMAILBODY + url;
        emailService.SendEmail(email, SENDMAILSUBJECT, url);
        strMessage = SENDMAILSUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public bool LockAccount(string id, bool status, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.User.Get(u => u.Email == id);
        if (data == null)
        {
          throw new BadRequestException(Constants.ACCOUNTNOTFOUND);
        }
        data.IsLocked = status;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        _unitOfWork.User.Update(data);
        _unitOfWork.Save();
        strMessage = Constants.ACCOUNTLOCKSUCCESS(status);
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string RefreshToken(UserRefreshToken user, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        if (_unitOfWork.User.ExistsBy(u => u.Email.Equals(user.UserName)))
        {
          strMessage = Constants.ACCOUNTNOTFOUNF;
          return null;
        }
        var oldClaims = _unitOfWork.User.GetClaimsFromToken(user.Token);
        var newAccessToken = _unitOfWork.User.GenerateWebToken(oldClaims);
        _unitOfWork.User.SetJWTCookie(newAccessToken);
        return newAccessToken;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public string GetUserEmail()
    {
      var context = _httpContextAccessor.HttpContext;
      if (context != null && context.Items["UserEmail"] != null)
      {
        return context.Items["UserEmail"].ToString();
      }

      return null;
    }


  }
}
