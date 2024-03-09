using API.Model;
using ICHI_API.Model;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
  public interface IUserService
  {
    string Register(UserRegister userRegister, out string strMessage);
    string Login(UserLogin userLogin, out string strMessage);
    string ForgotPassword(string email, out string strMessage);
    string RefreshToken(UserRefreshToken user, out string strMessage);
    string ChangePassword(UserChangePassword user, out string strMessage);
  }
}
