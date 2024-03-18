using API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_API.Model;
namespace ICHI_CORE.Controllers.MasterController
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(PcsApiContext context, IAuthService authService, IUserService userService, IConfiguration configuration = null)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<UserDTO>>>> GetAll(
                        [FromQuery(Name = "search")] string name = "",
                        [FromQuery(Name = "page-size")] int pageSize = 10,
                        [FromQuery(Name = "page-number")] int pageNumber = 1,
                        [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                        [FromQuery(Name = "sort-by")] string sortBy = "Email")
        {
            ApiResponse<ICHI_API.Helpers.PagedResult<UserDTO>> result;
            string strMessage = "";
            try
            {
                var data = _userService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<UserDTO>>(
                     System.Net.HttpStatusCode.OK,
                     strMessage,
                     data
                 );
            }
            catch (Exception ex)
            {
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<UserDTO>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        // đăng ký tài khoản
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ApiResponse<String>> Register([FromBody] UserRegister userRegister)
        {
            ApiResponse<String> result;
            string strMessage = string.Empty;
            try
            {
                string data = _authService.Register(userRegister, out strMessage);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
                return result;
            }
        }

        // Đăng nhập
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ApiResponse<String>> Login([FromBody] UserLogin userLogin)
        {
            ApiResponse<String> result;
            string strMessage = string.Empty;
            try
            {
                var data = _authService.Login(userLogin, out strMessage);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        // refresh token
        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> RefreshToken([FromBody] UserRefreshToken user)
        {
            ApiResponse<string> result;
            string strMessage = string.Empty;
            try
            {
                string data = _authService.RefreshToken(user, out strMessage);

                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }

            return result;
        }

        // viết hàm quên mật khẩu gửi về gmail
        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> ForgotPassword(string email)
        {
            ApiResponse<string> result;
            string strMessage = string.Empty;
            try
            {
                string data = _authService.ForgotPassword(email, out strMessage);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, null);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        // viết chức năng đổi mật khẩu
        [HttpPut]
        [Route("change-password")]
        public async Task<ApiResponse<string>> ChangePassword([FromBody] UserChangePassword user)
        {
            ApiResponse<string> result;
            string strMessage = string.Empty;
            try
            {
                string data = _authService.ChangePassword(user, out strMessage);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> Delete(string id, bool status)
        {
            string strMessage = "";
            try
            {
                var data = _authService.LockAccount(id, status, out strMessage);
                var result = new ApiResponse<Employee>(System.Net.HttpStatusCode.OK, strMessage, null);
                return Ok(result);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                return BadRequest(new ApiResponse<Employee>(System.Net.HttpStatusCode.BadRequest, strMessage, null));
            }
        }
    }
}
