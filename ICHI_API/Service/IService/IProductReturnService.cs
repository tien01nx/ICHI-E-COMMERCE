using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
    public interface IProductReturnService
    {
        PagedResult<ProductReturn> GetAll(string name, string status, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
        ProductReturnDTO Create(ProductReturnDTO model, out string strMessage);
        ProductReturnDTO Update(ProductReturnDTO model, out string strMessage);
        ProductReturnVM FindById(int id, out string strMessage);
    }
}
