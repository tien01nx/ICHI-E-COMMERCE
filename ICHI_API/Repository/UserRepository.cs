using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_CORE.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace ICHI.DataAccess.Repository
{
  using static ICHI_API.Helpers.Constants;
  public class UserRepository : Repository<User>, IUserRepository
  {
    private PcsApiContext _db;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRepository(PcsApiContext db, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(db)
    {
      _db = db;
      _configuration = configuration;
      _httpContextAccessor = httpContextAccessor;
    }

    public bool ExistsBy(Expression<Func<User, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public DateTime GetTokenExpirationTime(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.ValidTo ?? DateTime.MinValue;
    }

    // Refresh token
    public List<Claim> GetClaimsFromToken(string token)
    {
      var handler = new JwtSecurityTokenHandler();
      var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
      return jsonToken?.Claims.ToList() ?? new List<Claim>();
    }

    // Tạo token
    public string GenerateWebToken(List<Claim> claims)
    {

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      double expireHours = Convert.ToDouble(EXPIREDAY);
      var token = new JwtSecurityToken(
          issuer: ISSUER,
          audience: AUDIENCE,
          expires: DateTime.Now.AddDays(expireHours), // thời gian sống của token 
          signingCredentials: credentials,
          claims: claims.ToArray()
          );
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //  kiểm tra role của user
    public string GenerateAccessToken(User user)
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(SUB,user.Email.ToString())
            };
      var roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Email).Select(ur => ur.Role.RoleName).ToList();

      roles.ForEach(role => claims.Add(new Claim(ROLES, role)));

      var accessToken = GenerateWebToken(claims.ToList());

      return accessToken;
    }

    // Tạo mã token và lưu vào cookie
    public void SetJWTCookie(string token)
    {
      double expireHours = Convert.ToDouble(EXPIREDAY);

      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(expireHours),
      };
      _httpContextAccessor.HttpContext.Response.Cookies.Append(JWT, token, cookieOptions);
    }

    public void Update(User obj)
    {
      _db.Users.Update(obj);
    }
  }
}
