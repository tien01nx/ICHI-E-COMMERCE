using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;


namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductController : ControllerBase
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProductService _productService;
    public ProductController(PcsApiContext context, IProductService productService, IWebHostEnvironment webHostEnvironment)
    {
      _webHostEnvironment = webHostEnvironment;
      _productService = productService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>>>> GetAll(
            [FromQuery(Name = "search")] string name = "",
            [FromQuery(Name = "page-size")] int pageSize = 10,
            [FromQuery(Name = "page-number")] int pageNumber = 1,
            [FromQuery(Name = "sort-direction")] string sortDir = "desc",
            [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>> result;
      string strMessage = "";
      try
      {
        var data = _productService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>>(
                      System.Net.HttpStatusCode.OK,
                                 strMessage,
                                              data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<ProductDTO>>(ex);
      }
      return result;
    }

    // Lấy ra sản phẩm theo id
    [HttpGet("GetProductById/{id}")]
    public async Task<ApiResponse<ProductDTO>> GetProductById(int id)
    {
      ApiResponse<ProductDTO> result;
      string strMessage = "";
      try
      {
        var data = _productService.FindById(id, out strMessage);
        result = new ApiResponse<ProductDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ProductDTO>(ex);
      }
      return result;
    }
    [HttpPost("Upsert")]
    public async Task<ApiResponse<Product>> UpSert([FromForm] Product product, List<IFormFile>? files)
    {
      ApiResponse<Product> result;
      string strMessage = "";
      try
      {
        var data = _productService.Create(product, files, out strMessage);
        result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Product>(ex);
      }
      return result;
    }

    // xóa productimage theo productId và imageName
    [HttpDelete("Delete-Image")]
    public async Task<ApiResponse<ProductImages>> DeleteProductImage(int productId, string imageName)
    {
      ApiResponse<ProductImages> result;
      string strMessage = "";
      try
      {
        var data = _productService.DeleteProductImage(productId, imageName, out strMessage);
        result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.OK, strMessage, null);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ProductImages>(ex);
      }
      return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> Delete(int id)
    {
      string strMessage = "";
      try
      {

        //var data = await _productService.Products.FirstOrDefaultAsync(x => x.Id == id);
        var data = _productService.Delete(id, out strMessage);
        var result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Product>(ex);
      }
    }

    [HttpGet("FindProductInCategoryName")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>>>> GetProductInCategoryName(
         [FromQuery(Name = "category-name")] string categoryName = "",
         [FromQuery(Name = "category-parent")] string? category_parent = null,
         [FromQuery(Name = "colors")] string? colors = null,
         [FromQuery(Name = "trademark-name")] string? trademarkName = null,
         [FromQuery(Name = "price-min")] decimal? priceMin = null,
         [FromQuery(Name = "price-max")] decimal? priceMax = null,
         [FromQuery(Name = "page-size")] int pageSize = 10,
         [FromQuery(Name = "page-number")] int pageNumber = 1,
         [FromQuery(Name = "sort-direction")] string? sortDir = "desc",
         [FromQuery(Name = "sort-by")] string? sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>> result;
      string strMessage = "";
      try
      {

        var data = _productService.GetProductInCategory(categoryName, category_parent, colors, trademarkName, priceMin, priceMax, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>>(
                      System.Net.HttpStatusCode.OK,
                                 strMessage,
                                              data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<ProductDTO>>(ex);
      }
      return result;
    }
  }
}
