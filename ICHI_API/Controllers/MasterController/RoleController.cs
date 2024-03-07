using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using ICHI_CORE.Domain;
using ICHI_CORE.Model;
using System.Data;
using ICHI_CORE.Domain.MasterModel;
using ICHI.DataAccess.Data;

namespace ICHI_CORE.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RoleController : BaseController<Role>
  {
    public RoleController(PcsApiContext context) : base(context) { }

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
  }
}
