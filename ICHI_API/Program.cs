using ICHI.API.DbInitializer;
using ICHI_API.Data;
using ICHI_CORE.Extension;
using ICHI_CORE.Filter;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using NLog.Web;

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Config DBContext
  builder.Services.AddDbContext<PcsApiContext>(options => options.UseSqlServer(AppSettings.ConnectionString));
  builder.Services.AddIdentityServices(builder.Configuration);

  // Add services to the container.
  builder.Services.AddControllers();
  builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

  //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  // builder.Services.AddEndpointsApiExplorer();
  // builder.Services.AddSwaggerGen();

  // Config NLog
  builder.Logging.ClearProviders();
  builder.Host.UseNLog();

  // Get data from AppSettings
  IConfiguration configuration = new ConfigurationBuilder()
                              .AddJsonFile("appsettings.json")
                              .Build();
  if (AppSettings.InitData(configuration))
  {
    NLogger.log.Info("Get data from appSettings success.");
    NLogger.log.Error("Test log error");
  }
  else
  {
    NLogger.log.Error("Get data from appSettings error.");
  }
  builder.Services.AddLogging(builder =>
  {
    builder.Services.RemoveAll<ILoggerProvider>(); // Xóa tất cả các log tự động của hệ thống
  });
  var app = builder.Build();

  // Configure the HTTP request pipeline
  // // // tắt swagger.
  // if (app.Environment.IsDevelopment())
  // {
  //   app.UseSwagger();
  //   app.UseSwaggerUI();
  // }
  app.UseStaticFiles();
  app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
  //app.UseHttpsRedirection();

  app.UseRouting();
  //app.UseHttpsRedirection();
  app.UseMiddleware<JWTInHeaderMiddleware>();
  app.UseAuthentication();
  app.UseAuthorization();
  // Yêu cầu xác thực cho tất cả các endpoint
  //app.UseEndpoints(endpoints =>
  //{

  //  endpoints.MapControllers().RequireAuthorization();
  //});
  SeedDatabase();
  app.MapControllers();
  var websocketOptions = new WebSocketOptions
  {
    KeepAliveInterval = TimeSpan.FromMinutes(2)
  };
  app.UseWebSockets(websocketOptions);
  app.Run();
  void SeedDatabase()
  {
    using (var scope = app.Services.CreateScope())
    {
      var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
      dbInitializer.Initialize();
    }
  }
}
catch (Exception ex)
{
  NLogger.log.Error(ex.ToString());
  throw;
}
finally
{
  LogManager.Shutdown();
}
