using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using ICHI_CORE.Domain;
using ICHI_CORE.Model;
using System.Data;
using ICHI_API.Data;
using Microsoft.Data.SqlClient;
using ICHI_API.Model;
using ICHI_API.Service.IService;

namespace ICHI_CORE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController<User>
    {
        private readonly IUserService _userService;
        public UsersController(PcsApiContext context, IUserService userService) : base(context)
        {
            _userService = userService;
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
