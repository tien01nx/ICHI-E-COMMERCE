using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace ICHI_CORE.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(PcsApiContext context, IUserService userService)
    {
      _userService = userService;
    }

    public static string[] GenerateUrls()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var controllers = assembly.GetTypes()
          .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
          .Select(type => type.Name.Replace("Controller", ""))
          .ToList();

      var urls = controllers.SelectMany(controller =>
      {
        var controllerActions = assembly.GetTypes()
                  .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                  .Where(type => type.Name == $"{controller}Controller")
                  .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                  .Where(m => !m.GetCustomAttributes(typeof(NonActionAttribute)).Any())
                  .Select(m => $"/{controller}/{m.Name}");

        return controllerActions;
      }).ToArray();

      return urls;
    }

    [HttpGet]
    [Route("Get-All-Urls")]
    public async Task<ApiResponse<string[]>> GetAllUrls()
    {
      ApiResponse<string[]> result;
      try
      {
        var data = GenerateUrls();
        result = new ApiResponse<string[]>(System.Net.HttpStatusCode.OK, "", data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<string[]>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    [HttpGet]
    [Route("CheckConnect")]
    public async Task<ApiResponse<User>> CheckConnect()
    {
      ApiResponse<User> result;
      try
      {
        SqlConnection connection = new SqlConnection(AppSettings.ConnectionString);
        connection.Open();
        if (connection.State != ConnectionState.Open)
        {
          result = new ApiResponse<User>(System.Net.HttpStatusCode.ExpectationFailed, "Not connection to DB", null);
        }
        else
        {
          connection.Close();
          result = new ApiResponse<User>(System.Net.HttpStatusCode.OK, "", null);
        }

      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<User>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    // update user
    [HttpPut]
    [Route("Update-User")]
    public async Task<ApiResponse<UserDTO>> UpdateUser(UserDTO user)
    {
      ApiResponse<UserDTO> result;
      string strMessage = "";
      try
      {
        var data = _userService.UpdateAccount(user, out strMessage);
        result = new ApiResponse<UserDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        result = new ApiResponse<UserDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

  }
}
