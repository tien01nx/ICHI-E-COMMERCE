﻿using API.Model;
using ICHI_API.Model;
using ICHI_CORE.Domain;
using ICHI_CORE.Model;

namespace ICHI_API.Service.IService
{
    public interface IAuthService
    {
        string Login(UserLogin userLogin, out string strMessage);

        string Register(UserRegister userRegister, out string strMessage);

        string ForgotPassword(string email, out string strMessage);

        string ChangePassword(UserChangePassword user, out string strMessage);

        string LockAccount(string id, bool status, out string strMessage);

        string RefreshToken(UserRefreshToken user, out string strMessage);

        User ExistsByUserNameOrEmail(string email);

        bool ExistsByPhoneNumber(string phoneNumber);
    }
}