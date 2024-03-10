using API.Model;
using ICHI_API.Model;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace ICHI_API.Service.IService
{
  public interface IUserService
  {
    string Register(UserRegister userRegister, out string strMessage);
    string Login(UserLogin userLogin, out string strMessage);
    string ForgotPassword(string email, out string strMessage);
    string RefreshToken(UserRefreshToken user, out string strMessage);
    string ChangePassword(UserChangePassword user, out string strMessage);
    Helpers.PagedResult<UserDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    string LockAccount(int id, bool status, out string strMessage);
    UserDTO UpdateAccount(UserDTO user, out string strMessage);
  }
}
