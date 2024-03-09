using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
  public interface ICustomerService
  {
    PagedResult<Customer> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    Customer Create(Customer supplier, out string strMessage);
    Customer Update(Customer supplier, out string strMessage);
    Customer FindById(int id, out string strMessage);
    bool Delete(int id, out string strMessage);
  }
}
