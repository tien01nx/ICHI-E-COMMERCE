using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_CORE.NlogConfig;
using ICHI_API;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_API.Model;
using ICHI_API.Service;
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
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<InventoryReceipt>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<List<Product>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<InventoryReceiptDTO>> CreateSupplỉer([FromBody] InventoryReceiptDTO model)
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpPost("Update")]
        public async Task<ApiResponse<InventoryReceiptDTO>> UpdateInventory([FromBody] InventoryReceiptDTO model)
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
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                result = new ApiResponse<InventoryReceiptDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        //[HttpPut("Update")]
        //public async Task<ApiResponse<InventoryReceipt>> UpdateSupplỉer([FromBody] InventoryReceipt supplier)
        //{
        //  ApiResponse<InventoryReceipt> result;
        //  string strMessage = "";
        //  try
        //  {
        //    var data = _inventoryService.Update(supplier, out strMessage);
        //    result = new ApiResponse<InventoryReceipt>(System.Net.HttpStatusCode.OK, strMessage, data);
        //  }
        //  catch (Exception ex)
        //  {
        //    result = new ApiResponse<InventoryReceipt>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ApiResponse<InventoryReceipt>>> Delete(int id)
        //{
        //  string strMessage = "";
        //  try
        //  {
        //    var data = _inventoryService.Delete(id, out strMessage);
        //    var result = new ApiResponse<InventoryReceipt>(System.Net.HttpStatusCode.OK, strMessage, null);
        //    return Ok(result);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error(ex.ToString());
        //    strMessage = "Có lỗi xảy ra";
        //    return BadRequest(new ApiResponse<InventoryReceipt>(System.Net.HttpStatusCode.BadRequest, strMessage, null));
        //  }
        //}
    }
}
