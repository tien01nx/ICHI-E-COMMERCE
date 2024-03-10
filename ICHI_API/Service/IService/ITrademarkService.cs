using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
  public interface ITrademarkService
  {
    Helpers.PagedResult<Trademark> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    Trademark Create(Trademark supplier, out string strMessage);
    Trademark Update(Trademark supplier, out string strMessage);
    Trademark FindById(int id, out string strMessage);
    bool Delete(int id, out string strMessage);

  }
}
