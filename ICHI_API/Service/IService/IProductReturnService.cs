using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
  public interface IProductReturnService
  {
    PagedResult<ProductReturn> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    ProductReturnDTO Create(ProductReturnDTO model, out string strMessage);
    ProductReturnDTO Update(ProductReturnDTO model, out string strMessage);
    ProductReturnDetail FindById(int id, out string strMessage);
  }
}
