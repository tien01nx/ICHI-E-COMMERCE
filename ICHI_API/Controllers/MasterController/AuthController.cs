using API.Helpers;
using API.Model;
using ICHI_CORE.Domain;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.EntityFrameworkCore;

namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : BaseController<User>
  {
    private readonly IConfiguration _configuration;
    public AuthController(PcsApiContext context, IConfiguration configuration = null) : base(context)
    {
      _configuration = configuration;
    }

    //[HttpPost]
    //[AllowAnonymous]
    //[Route("Register")]
    //public async Task<ApiResponse<String>> Register([FromBody] UserRegister userRegister)
    //{
    //  ApiResponse<String> result;

    //  try
    //  {
    //    var userExists = await _context.Users.AnyAsync(a => a.UserName.ToLower().Equals(userRegister.UserName.ToLower()));
    //    if (userExists)
    //    {
    //      result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Tên tài khoản đã tồn tại", null);
    //      return result;
    //    }
    //    var phoneCustomerExists = await _context.Customers.AnyAsync(a => a.PhoneNumber == userRegister.PhoneNumber);
    //    var phoneEmployeeExists = await _context.Employees.AnyAsync(a => a.PhoneNumber == userRegister.PhoneNumber);

    //    if (phoneCustomerExists && phoneEmployeeExists)
    //    {
    //      result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Số điện thoại đã tồn tại", null);
    //      return result;
    //    }

    //    User user = new User();
    //    MapperHelper.Map<UserRegister, User>(userRegister, user);
    //    string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password, salt);
    //    user.Password = hashedPassword;
    //    user.IsLocked = false;
    //    user.CreateBy = userRegister.UserName;
    //    user.ModifiedBy = userRegister.UserName;
    //    user.UserName = userRegister.UserName.Trim();
    //    var userResponse = await Create(user);
    //    await _context.SaveChangesAsync();

    //    var Role = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == AppSettings.USER);
    //    UserRole userRole = new UserRole
    //    {
    //      RoleId = Role.Id,
    //      UserId = user.Id
    //    };

    //    var userRoleResponse = await _context.UserRoles.AddAsync(userRole);
    //    await _context.SaveChangesAsync();

    //    // insert vào customer
    //    Customer customer = new Customer()
    //    {
    //      PhoneNumber = userRegister.PhoneNumber,
    //      FullName = userRegister.FullName,
    //      UserID = user.Id,
    //    };
    //    var customerResponse = await _context.Customers.AddAsync(customer);
    //    await _context.SaveChangesAsync();

    //    var accessToken = await GenerateAccessToken(user);
    //    SetJWTCookie(accessToken);

    //    return new ApiResponse<string>(System.Net.HttpStatusCode.OK, "Đăng ký tài khoản thành công", accessToken);
    //  }
    //  catch (Exception ex)
    //  {
    //    NLogger.log.Error(ex.ToString());
    //    result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
    //  }
    //  return result;
    //}


    [HttpPost]
    [AllowAnonymous]
    [Route("Register")]
    public async Task<ApiResponse<String>> Register([FromBody] UserRegister userRegister)
    {
      ApiResponse<String> result;

      try
      {
        using (var transaction = _context.Database.BeginTransaction())
        {
          var userExists = await _context.Users.AnyAsync(a => a.UserName.ToLower().Equals(userRegister.UserName.ToLower()));
          if (userExists)
          {
            result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Tên tài khoản đã tồn tại", null);
            return result;
          }
          var phoneCustomerExists = await _context.Customers.AnyAsync(a => a.PhoneNumber == userRegister.PhoneNumber);
          var phoneEmployeeExists = await _context.Employees.AnyAsync(a => a.PhoneNumber == userRegister.PhoneNumber);

          if (phoneCustomerExists && phoneEmployeeExists)
          {
            result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Số điện thoại đã tồn tại", null);
            return result;
          }

          User user = new User();
          MapperHelper.Map<UserRegister, User>(userRegister, user);
          string salt = BCrypt.Net.BCrypt.GenerateSalt();
          string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password, salt);
          user.Password = hashedPassword;
          user.IsLocked = false;
          user.CreateBy = userRegister.UserName;
          user.ModifiedBy = userRegister.UserName;
          user.UserName = userRegister.UserName.Trim();
          var userResponse = await Create(user);

          var Role = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == AppSettings.USER);
          UserRole userRole = new UserRole
          {
            RoleId = Role.Id,
            UserId = user.Id
          };

          var userRoleResponse = await _context.UserRoles.AddAsync(userRole);

          // insert vào customer
          Customer customer = new Customer()
          {
            PhoneNumber = userRegister.PhoneNumber,
            FullName = userRegister.FullName,
            UserID = user.Id,
          };
          var customerResponse = await _context.Customers.AddAsync(customer);

          // Thực hiện commit transaction
          await _context.SaveChangesAsync();
          transaction.Commit();

          var accessToken = await GenerateAccessToken(user);
          SetJWTCookie(accessToken);

          return new ApiResponse<string>(System.Net.HttpStatusCode.OK, "Đăng ký tài khoản thành công", accessToken);
        }
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, "Có lỗi xảy ra", null);

        return result;
      }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Login")]
    public async Task<ApiResponse<String>> Login([FromBody] UserLogin userLogin)
    {
      ApiResponse<String> result;

      try
      {
        User loginUser = GetUserByUsername(userLogin.UserName.Trim());



        if (loginUser == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, loginUser.Password))
        {
          result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Tài khoản hoặc mật khẩu không đúng", null);
          return result;
        }

        var accessToken = await GenerateAccessToken(loginUser);
        SetJWTCookie(accessToken);

        return new ApiResponse<string>(System.Net.HttpStatusCode.OK, "Đăng nhập thành công", accessToken);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, "Có lỗi xảy ra", null);
      }
      return result;
    }

    [HttpPost]
    [Route("refresh-token")]
    [AllowAnonymous]
    public async Task<ApiResponse<string>> RefreshToken([FromBody] UserRefreshToken user)
    {
      ApiResponse<string> result;

      try
      {
        User loginUser = GetUserByUsername(user.UserName);

        if (loginUser == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, loginUser.Password))
        {
          result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Có lỗi xảy ra khi lất token", null);
          return result;
        }

        var oldClaims = GetClaimsFromToken(user.Token);
        var newAccessToken = GenerateWebToken(oldClaims);
        SetJWTCookie(newAccessToken);

        return new ApiResponse<string>(System.Net.HttpStatusCode.OK, "", newAccessToken);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }

      return result;
    }
    private async Task<string> GenerateAccessToken(User user)
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
      var roles = await _context.UserRoles
          .Where(ur => ur.UserId == user.Id)
          .Join(_context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => role.RoleName)
          .ToListAsync();

      roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

      var accessToken = GenerateWebToken(claims.ToList());
      return accessToken;
    }

    private User GetUserByUsername(string username)
    {
      //var usersResponse = FindAll();

      //if (usersResponse.Result.Code != System.Net.HttpStatusCode.OK)
      //{
      //  return null;
      //}

      //var users = usersResponse.Result.Data;
      //return users.FirstOrDefault(a => a.UserName.ToLower().Equals(username.ToLower()));
      var user = _context.Users.FirstOrDefault(a => a.UserName.ToLower().Equals(username.ToLower()));
      if (user == null)
      {
        return null;
      }
      return user;
    }

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
    private void SetJWTCookie(string token)
    {
      double expireHours = Convert.ToDouble(_configuration["Jwt:ExpireDay"]);

      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(expireHours), // thời gian sống của cookie 
      };
      Response.Cookies.Append("Jwt", token, cookieOptions);
    }

    private List<Claim> GetClaimsFromToken(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.Claims.ToList() ?? new List<Claim>();
    }

    // kiểm tra xem token có hết hạn hay không
    private DateTime GetTokenExpirationTime(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.ValidTo ?? DateTime.MinValue;
    }
  }
}
