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
    PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
    PagedResult<ProductDTO> GetProductInCategory(string categoryName, int pageSize, int pageNumber, string sortDir, string sortBy, List<string>? colors, List<string>? trademarkName, decimal? priceMin, decimal? priceMax, out string strMessage);

    IQueryable<Product> FilterProducts(IQueryable<Product> query, string categoryName, List<string> color, List<string> trademark, decimal? priceMin, decimal? priceMax);

    Product Create(Product product, List<IFormFile>? files, out string strMessage);

    ProductDTO FindById(int id, out string strMessage);

    bool Delete(int id, out string strMessage);

    bool DeleteProductImage(int productId, string imageName, out string strMessage);



  }
}
