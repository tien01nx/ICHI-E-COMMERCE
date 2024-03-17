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

        Helpers.PagedResult<UserDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);


        UserDTO UpdateAccount(UserDTO user, out string strMessage);
    }
}
