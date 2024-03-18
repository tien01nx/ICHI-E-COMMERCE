using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using System.Net.Http.Headers;
using ICHI_CORE.Model;
using System.Linq.Dynamic.Core;
using ICHI_CORE.Helpers;
using ICHI_API.Model;
using Microsoft.AspNetCore.Hosting;
using ICHI_API;
using Microsoft.EntityFrameworkCore;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using System.Net.WebSockets;
using ICHI_CORE.NlogConfig;


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
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<ProductDTO>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<ProductDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<Product>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ApiResponse<Product>>> Delete(int id)
        //{
        //  try
        //  {
        //    var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        //    data.isDeleted = true;
        //    data.ModifiedDate = DateTime.Now;
        //    data.ModifiedBy = "Admin";
        //    await Update(data);
        //    var result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "", data);
        //    return Ok(result);
        //  }
        //  catch (Exception ex)
        //  {
        //    return BadRequest(new ApiResponse<Product>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
        //  }
        //}
    }

}
