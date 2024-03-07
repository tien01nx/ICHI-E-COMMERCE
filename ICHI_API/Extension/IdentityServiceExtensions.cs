
using ICHI.DataAccess.DbInitializer;
using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace ICHI_CORE.Extension
{
  public static class IdentityServiceExtensions
  {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddAuthentication(option =>
      {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidAudience = config["Jwt:Audience"],
          ValidIssuer = config["Jwt:Issuer"],
          ClockSkew = TimeSpan.Zero,
          IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
        };
      });
      services.AddScoped<IDbInitializer, DbInitializer>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      return services;
    }
  }
}
