using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace ICHI.DataAccess.Repository
{
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

        // Đăng  ký tài khoản

        // kiểm tra user có tồn tại không theo username hoặc email
        public User ExistsByUserNameOrEmail(string userName)
        {
            return dbSet.FirstOrDefault(x => x.Email.Equals(userName));
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
        private async Task<string> GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };
            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == user.Email)
                .Join(_db.Roles,
                      userRole => userRole.RoleId,
                      role => role.Id,
                      (userRole, role) => role.RoleName)
                .ToListAsync();

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
        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
    }
}
