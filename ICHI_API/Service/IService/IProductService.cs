using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
    public interface IProductService
    {
        PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
        PagedResult<ProductDTO> GetProductInCategory(string categoryName, string? category_parent, string? color, string? trademarkName, decimal? priceMin, decimal? priceMax, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);

        Product Create(Product product, List<IFormFile>? files, out string strMessage);

        ProductDTO FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);

        bool DeleteProductImage(int productId, string imageName, out string strMessage);

        List<ProductDTO> ProductTopFive(string dateTime, out string strMessage);
    }
}
