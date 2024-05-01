using API.Model;
using ICHI_API.Model;
using ICHI_CORE.Model;

namespace ICHI_API.Service.IService
{
  public interface IAuthService
  {
    string Login(UserLogin userLogin, out string strMessage);

    string RegisterCustomer(UserRegister userRegister, out string strMessage);
    string RegisterEmployee(UserRegister userRegister, out string strMessage);

    bool ForgotPassword(string email, out string strMessage);

    bool ChangePassword(UserChangePassword user, out string strMessage);

    bool LockAccount(string id, bool status, out string strMessage);

    string RefreshToken(UserRefreshToken user, out string strMessage);
    string GetUserEmail();
  }
}
