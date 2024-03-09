using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
  public interface ISupplierService
  {
    PagedResult<Supplier> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    Supplier Create(Supplier supplier, out string strMessage);
    Supplier Update(Supplier supplier, out string strMessage);
    Supplier FindById(int id, out string strMessage);
    bool Delete(int id, out string strMessage);

  }
}
