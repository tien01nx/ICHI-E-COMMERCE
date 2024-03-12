using API.Model;
using Azure.Core;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Extension;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq.Dynamic.Core;

namespace ICHI_API.Service
{
  public class UserService : IUserService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PcsApiContext _db;
    public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _configuration = configuration;
      _httpContextAccessor = httpContextAccessor;
      _db = pcsApiContext;
    }
    public string Register(UserRegister userRegister, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        using (var transaction = _db.Database.BeginTransaction())
        {
          // kiểm tra người dùng có hợp lệ không
          if (ExistsByPhoneNumber(userRegister.PhoneNumber.Trim()))
          {
            strMessage = "Số điện thoại đã tồn tại";
            return null;
          }
          if (ExistsByUserNameOrEmail(userRegister.Email.Trim()) != null)
          {
            strMessage = "User đã tồn tại";
            return null;
          }
          // mật khẩu phải > 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt

          if (!userRegister.Password.Any(char.IsUpper) || !userRegister.Password.Any(char.IsLower) || !userRegister.Password.Any(char.IsDigit) || userRegister.Password.Length < 8)
          {
            strMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt";
            return null;
          }
          User user = new User();
          MapperHelper.Map<UserRegister, User>(userRegister, user);
          string salt = BCrypt.Net.BCrypt.GenerateSalt();
          string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password.Trim(), salt);
          user.Password = hashedPassword;
          user.IsLocked = false;
          user.Avatar = AppSettings.AvatarDefault;
          user.CreateBy = userRegister.Email;
          user.ModifiedBy = userRegister.Email;
          user.Email = userRegister.Email.Trim();
          _unitOfWork.User.Add(user);
          _unitOfWork.Save();
          if (userRegister.Role == AppSettings.USER)
          {
            // insert vào customer
            Customer customer = new Customer()
            {
              PhoneNumber = userRegister.PhoneNumber,
              FullName = userRegister.FullName,
              UserId = user.Id,
              Gender = userRegister.Gender,
              Birthday = userRegister.Birthday,
            };
            _unitOfWork.Customer.Add(customer);
            _unitOfWork.Save();
          }
          else
          {
            // insert vào employee
            Employee employee = new Employee()
            {
              PhoneNumber = userRegister.PhoneNumber,
              FullName = userRegister.FullName,
              UserId = user.Id,
              Gender = userRegister.Gender,
              Birthday = userRegister.Birthday,

            };
            _unitOfWork.Employee.Add(employee);
            _unitOfWork.Save();
          }
          transaction.Commit();
          strMessage = "Đăng ký thành công";
          var accessToken = GenerateAccessToken(user);
          SetJWTCookie(accessToken);
          return accessToken;
        }
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        return null;
      }
    }

    public string Login(UserLogin userLogin, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        User loginUser = ExistsByUserNameOrEmail(userLogin.UserName);
        if (loginUser == null)
        {
          strMessage = "Tài khoản không tồn tại";
          return null;
        }
        // nếu loginuser tồn tại thì kiểm tra mật khẩu
        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loginUser.Password))
        {
          // nếu người dùng đăng nhập sai thì thực hiện cập nhật số lần đăng nhập sai
          loginUser.FailedPassAttemptCount++;
          // nếu đăng nhập sai quá 5 lần thì khóa tài khoản
          if (loginUser.FailedPassAttemptCount >= 5)
          {
            loginUser.IsLocked = true;
            strMessage = "Tài khoản đã bị khóa";
          }
          else
          {
            strMessage = "Mật khẩu không đúng";
          }
          _unitOfWork.User.Update(loginUser);
          _unitOfWork.Save();
          return null;
        }
        strMessage = "Đăng nhập thành công";
        var accessToken = GenerateAccessToken(loginUser);
        SetJWTCookie(accessToken);
        return accessToken;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }


    public string ForgotPassword(string email, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        User loginUser = ExistsByUserNameOrEmail(email);
        if (loginUser == null)
        {
          strMessage = "Tài khoản không tồn tại";
          return null;
        }
        var emailService = new EmailService(_configuration);
        // random mật khẩu mới
        Random random = new Random();
        string randomString = new string(Enumerable.Repeat(AppSettings.Encode, random.Next(8, 16))
                   .Select(s => s[random.Next(s.Length)]).ToArray());
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomString, salt);
        User user = _unitOfWork.User.Get(u => u.Email == loginUser.Email || u.Email.Equals(email));
        user.Password = hashedPassword;
        user.ModifiedBy = loginUser.Email;
        user.ModifiedDate = DateTime.Now;
        _unitOfWork.User.Update(user);
        _unitOfWork.Save();
        string url = "Mật khẩu mới của bạn là: " + randomString;
        string body = "Click vào link sau để đổi mật khẩu: " + url;
        emailService.SendEmail(email, "Reset password", url);
        strMessage = "Gửi email thành công";
        return "Gửi email thành công";
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }

    }

    public string RefreshToken(UserRefreshToken user, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // check người dùng có hợp lệ không
        User loginUser = ExistsByUserNameOrEmail(user.UserName);
        if (loginUser == null)
        {
          strMessage = "Tài khoản không tồn tại";
          return null;
        }
        var oldClaims = GetClaimsFromToken(user.Token);
        var newAccessToken = GenerateWebToken(oldClaims);
        SetJWTCookie(newAccessToken);
        return newAccessToken;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    // kiểm tra user có tồn tại không theo username
    public User ExistsByUserNameOrEmail(string email)
    {
      var data = _unitOfWork.User.Get(u => u.Email.ToLower().Equals(email) || u.Email.Equals(email));
      if (data != null)
      {
        return data;
      }
      return null;
    }

    // kiểm tra theo phonenumber ở bảng customer và employee
    public bool ExistsByPhoneNumber(string phoneNumber)
    {
      return _unitOfWork.Customer.Get(c => c.PhoneNumber.Equals(phoneNumber)) != null || _unitOfWork.Employee.Get(e => e.PhoneNumber.Equals(phoneNumber)) != null;
    }

    // kiểm tra token có hết hạn chưa
    private DateTime GetTokenExpirationTime(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.ValidTo ?? DateTime.MinValue;
    }
    // Refresh token
    private List<Claim> GetClaimsFromToken(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.Claims.ToList() ?? new List<Claim>();
    }

    // Tạo token
    private string GenerateWebToken(List<Claim> claims)
    {

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      double expireHours = Convert.ToDouble(_configuration["Jwt:ExpireDay"]);
      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          expires: DateTime.Now.AddDays(expireHours), // thời gian sống của token 
          signingCredentials: credentials,
          claims: claims.ToArray()
          );
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //  kiểm tra role của user
    private string GenerateAccessToken(User user)
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };
      var roles = _unitOfWork.UserRole.GetAll(ur => ur.UserId == user.Id, includeProperties: "Role").Select(ur => ur.Role.RoleName).ToList();

      roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

      var accessToken = GenerateWebToken(claims.ToList());
      return accessToken;
    }

    // Tạo mã token và lưu vào cookie
    public void SetJWTCookie(string token)
    {
      double expireHours = Convert.ToDouble(_configuration["Jwt:ExpireDay"]);

      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(expireHours),
      };
      _httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt", token, cookieOptions);
    }

    public string ChangePassword(UserChangePassword user, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var userExist = ExistsByUserNameOrEmail(user.UserName);
        if (userExist != null && BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password))
        {
          string salt = BCrypt.Net.BCrypt.GenerateSalt();
          string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.NewPassword, salt);
          userExist.Password = hashedPassword;
          userExist.ModifiedBy = user.UserName;
          userExist.ModifiedDate = DateTime.Now;
          _unitOfWork.User.Update(userExist);
          _unitOfWork.Save();
          return "Đổi mật khẩu thành công";
        }
        else
        {
          strMessage = "Mật khẩu cũ không đúng";
          return null;
        }
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Helpers.PagedResult<UserDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _unitOfWork.UserRole.GetAll(ur => ur.User != null, includeProperties: "User,Role");
        var userDTOs = query.Select(ur => new UserDTO
        {
          User = ur.User,
          Role = ur.Role != null ? ur.Role.RoleName : "",
          Email = ur.User.Email,
          Password = ur.User.Password,
        }).ToList();

        foreach (var userDTO in userDTOs)
        {
          if (userDTO.Role == AppSettings.USER)
          {
            var customer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.User.Id);
            if (customer == null)
            {
              continue;
            }
            userDTO.FullName = customer.FullName;
            userDTO.Birthday = customer.Birthday;
            userDTO.Gender = customer.Gender;
          }
          else if (userDTO.Role == AppSettings.EMPLOYEE)
          {
            var employee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.User.Id);
            if (employee == null)
            {
              continue;
            }
            userDTO.FullName = employee.FullName;
            userDTO.Birthday = employee.Birthday;
            userDTO.Gender = employee.Gender;
          }
        }

        if (!string.IsNullOrEmpty(name))
        {
          userDTOs = userDTOs.Where(u => u.Email.Contains(name)).ToList();
        }

        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        userDTOs = userDTOs.AsQueryable().OrderBy(orderBy).ToList();
        var pagedResult = Helpers.PagedResult<UserDTO>.CreatePagedResult(userDTOs.AsQueryable(), pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }


    public string LockAccount(int id, bool status, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.User.Get(u => u.Id == id);
        if (data == null)
        {
          strMessage = "Tài khoản không tồn tại";
          return null;
        }
        data.IsLocked = status;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        _unitOfWork.User.Update(data);
        _unitOfWork.Save();
        strMessage = status ? "Mở khóa tài khoản thành công" : "Khóa tài khoản thành công";
        return null;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public UserDTO UpdateAccount(UserDTO userDTO, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var userRole = _unitOfWork.UserRole.Get(ur => ur.UserId == userDTO.Id, "User");
        var userRoleCu = _unitOfWork.UserRole.Get(ur => ur.UserId == userDTO.Id, "User,Role");
        if (userRole == null)
        {
          strMessage = "Tài khoản không tồn tại";
          return null;
        }
        // so sánh tên role người dùng truyền lên với role trong db
        if (userRoleCu.Role.RoleName != userDTO.Role)
        {
          var role = _unitOfWork.Role.Get(r => r.RoleName == userDTO.Role);
          if (role == null)
          {
            strMessage = "Role không tồn tại";
            return null;
          }
          userRole.RoleId = role.Id;
          _unitOfWork.UserRole.Update(userRole);
          _unitOfWork.Save();
        }
        // kiểm tra userId tồn tại ở bảng nào thì update vào bảng đó là customner hoặc employee kiểm trả về là bool để xem ở bảng nào
        var isCustomer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.Id) != null;
        if (isCustomer)
        {
          var customer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.Id);
          if (customer == null)
          {
            strMessage = "Khách hàng không tồn tại";
            return null;
          }
          customer.FullName = userDTO.FullName;
          customer.Gender = userDTO.Gender;
          customer.Birthday = userDTO.Birthday;
          _unitOfWork.Customer.Update(customer);
        }
        var isEmployee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.Id) != null;
        if (isEmployee)
        {
          var employee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.Id);
          if (employee == null)
          {
            strMessage = "Nhân viên không tồn tại";
            return null;
          }
          employee.FullName = userDTO.FullName;
          employee.Gender = userDTO.Gender;
          employee.Birthday = userDTO.Birthday;
          _unitOfWork.Employee.Update(employee);
        }
        // update user khi thay đổi email
        if (!userRoleCu.User.Email.ToLower().Equals(userDTO.Email))
        {
          var userdb = _unitOfWork.User.Get(u => u.Id == userRoleCu.UserId);
          // kiểm tra email có tồn tại không
          var user = _unitOfWork.User.Get(u => u.Email.ToLower().Equals(userDTO.Email) && u.Id != userRoleCu.User.Id);
          if (user != null)
          {
            strMessage = "Email đã tồn tại";
            return null;
          }
          userdb.Email = userDTO.Email;
          _unitOfWork.User.Update(userdb);
        }
        _unitOfWork.Save();
        return userDTO;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
  }
}
