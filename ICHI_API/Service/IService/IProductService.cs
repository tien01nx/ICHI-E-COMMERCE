using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
  public interface IProductService
  {
    PagedResult<Product> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    Product Create(Product product, List<IFormFile>? files, out string strMessage);
    ProductDTO FindById(int id, out string strMessage);
    bool Delete(int id, out string strMessage);
    bool DeleteProductImage(int productId, string imageName, out string strMessage);
  }
}
