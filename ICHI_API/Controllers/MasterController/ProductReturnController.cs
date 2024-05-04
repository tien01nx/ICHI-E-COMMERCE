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
    public class ProductReturnController : ControllerBase
    {
        private readonly IProductReturnService _productReturnService;
        public ProductReturnController(PcsApiContext context, IProductReturnService productReturnService)
        {
            _productReturnService = productReturnService;
        }

        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<ProductReturn>>>> GetAll(
                        [FromQuery(Name = "search")] string name = "",
                        [FromQuery(Name = "status")] string status = "",
                        [FromQuery(Name = "page-size")] int pageSize = 10,
                        [FromQuery(Name = "page-number")] int pageNumber = 1,
                        [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                        [FromQuery(Name = "sort-by")] string sortBy = "Id")
        {
            ApiResponse<ICHI_API.Helpers.PagedResult<ProductReturn>> result;
            string strMessage = "";
            try
            {
                var data = _productReturnService.GetAll(name, status, pageSize, pageNumber, sortDir, sortBy, out strMessage);
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<ProductReturn>>(
                     System.Net.HttpStatusCode.OK,
                     "Retrieved successfully",
                     data
                 );

            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<ICHI_API.Helpers.PagedResult<ProductReturn>>(ex);
            }
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductReturnVM>>> FindById(int id)
        {
            ApiResponse<ProductReturnVM> result;
            string strMessage = "";
            try
            {
                var data = _productReturnService.FindById(id, out strMessage);
                result = new ApiResponse<ProductReturnVM>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<ProductReturnVM>(ex);
            }
            return result;
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<ProductReturnDTO>> Create([FromBody] ProductReturnDTO model)
        {
            ApiResponse<ProductReturnDTO> result;
            string strMessage = "";

            //if (model.modelDetails.Count() == 0)
            //{
            //  strMessage = "Yêu cầu phải có sản phẩm";
            //  return new ApiResponse<ProductReturnDTO>(System.Net.HttpStatusCode.BadRequest, strMessage, null);
            //}
            try
            {
                var data = _productReturnService.Create(model, out strMessage);
                result = new ApiResponse<ProductReturnDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<ProductReturnDTO>(ex);
            }
            return result;
        }
        [HttpPut("Update")]
        public async Task<ApiResponse<ProductReturnDTO>> Update([FromBody] ProductReturnDTO model)
        {
            ApiResponse<ProductReturnDTO> result;
            string strMessage = "";
            try
            {
                var data = _productReturnService.Update(model, out strMessage);
                result = new ApiResponse<ProductReturnDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<ProductReturnDTO>(ex);
            }
            return result;
        }
    }
}
