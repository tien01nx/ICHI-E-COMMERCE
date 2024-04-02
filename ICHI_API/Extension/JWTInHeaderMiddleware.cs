using ICHI_CORE.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ICHI_CORE.Filter
{
  public class JWTInHeaderMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public JWTInHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
    {
      _next = next;
      _configuration = configuration;
    }
    //public async Task Invoke(HttpContext context)
    //{
    //    var name = "Jwt";
    //    var cookie = context.Request.Cookies[name];
    //    if (cookie != null)
    //    {
    //        if (!context.Request.Headers.ContainsKey("Authorization"))
    //            context.Request.Headers.Append("Authorization", "Bearer " + cookie);
    //    }
    //    await _next(context);
    //}


    public async Task Invoke(HttpContext context)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      if (token != null)
      {
        var email = GetEmailFromToken(token, _configuration);
        if (!string.IsNullOrEmpty(email))
        {
          context.Items["UserEmail"] = email;
        }
      }
      await _next(context);
    }

    private static string GetEmailFromToken(string token, IConfiguration configuration)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]); // Thay thế bằng mã key thực tế từ cấu hình
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidIssuer = "https://localhost:7287/",
          ValidAudience = "https://localhost:7287/",
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        return email;
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }
  }
}
