
using ICHI.API.DbInitializer;
using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Service;
using ICHI_API.Service.IService;
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
      services.AddScoped<IDbInitializer, ICHI.API.DbInitializer.DbInitializer>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<ISupplierService, SupplierService>();
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<ICustomerService, CustomerService>();
      services.AddScoped<IEmployeeService, EmployeeService>();
      services.AddScoped<ITrademarkService, TrademarkService>();
      services.AddScoped<ICategoryProductService, CategoryProductService>();
      services.AddScoped<IPromotionService, PromotionService>();
      services.AddScoped<ITrxTransactionService, TrxTransactionService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICartService, CartService>();
      services.AddScoped<IVnPayService, VnPayService>();
      services.AddScoped<IInventoryReceiptService, InventoryReceiptService>();
      services.AddScoped<IProductReturnService, ProductReturnService>();

      return services;
    }
  }
}
