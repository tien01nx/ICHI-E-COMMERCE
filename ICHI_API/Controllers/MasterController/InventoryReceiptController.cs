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
    public class InventoryReceiptController : ControllerBase
    {
        private readonly IInventoryReceiptService _inventoryService;

        public InventoryReceiptController(PcsApiContext context, IInventoryReceiptService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<InventoryReceipt>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "asc",
                    [FromQuery(Name = "sort-by")] string sortBy = "CreateDate")
        {
            ApiResponse<ICHI_API.Helpers.PagedResult<InventoryReceipt>> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<InventoryReceipt>>(
                     System.Net.HttpStatusCode.OK,
                     "Retrieved successfully",
                     data
                 );

            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<ICHI_API.Helpers.PagedResult<InventoryReceipt>>(ex);
            }
            return result;
        }

        [HttpGet("FindAll")]
        public async Task<ApiResponse<List<Product>>> FindAll()
        {
            ApiResponse<List<Product>> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.GetProductWithBatchNumber(out strMessage);
                result = new ApiResponse<List<Product>>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<List<Product>>(ex);
            }
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<InventoryReceiptDTO>>> FindById(int id)
        {
            ApiResponse<InventoryReceiptDTO> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.FindById(id, out strMessage);
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<InventoryReceiptDTO>(ex);
            }
            return result;
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<InventoryReceiptDTO>> Create([FromBody] InventoryReceiptDTO model)
        {
            ApiResponse<InventoryReceiptDTO> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.Create(model, out strMessage);
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.OK, strMessage, model);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<InventoryReceiptDTO>(ex);
            }
            return result;
        }

        [HttpPost("Update")]
        public async Task<ApiResponse<InventoryReceiptDTO>> Update([FromBody] InventoryReceiptDTO model)
        {
            ApiResponse<InventoryReceiptDTO> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.Update(model, out strMessage);
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.OK, strMessage, model);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<InventoryReceiptDTO>(ex);
            }
            return result;
        }

        [HttpGet("InventoryModels")]
        public async Task<ActionResult<ApiResponse<List<InventoryModel>>>> InventoryModels(int year)
        {
            ApiResponse<List<InventoryModel>> result;
            string strMessage = "";
            try
            {
                var data = _inventoryService.InventoryModels(year, out strMessage);
                result = new ApiResponse<List<InventoryModel>>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                var handler = new GlobalExceptionHandler();
                return handler.HandleException<List<InventoryModel>>(ex);
            }
            return result;
        }

    }
}
