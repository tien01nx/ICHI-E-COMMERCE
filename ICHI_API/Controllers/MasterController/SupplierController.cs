using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_API;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.NlogConfig;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class SupplierController : BaseController<Supplier>
  {
    private readonly ISupplierService _supplierService;
    public SupplierController(PcsApiContext context, ISupplierService supplierService) : base(context)
    {
      _supplierService = supplierService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>> result;
      string strMessage = "";
      try
      {
        var data = _supplierService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             data
         );

      }
      catch (Exception ex)
      {
        strMessage = "Có lỗi xảy ra";
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Supplier>>> FindById(int id)
    {
      ApiResponse<Supplier> result;
      string strMessage = "";
      try
      {
        var data = _supplierService.FindById(id, out strMessage);
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

    [HttpPost("Create-Supplier")]
    public async Task<ApiResponse<Supplier>> CreateSupplỉer([FromBody] Supplier supplier)
    {
      ApiResponse<Supplier> result;
      string strMessage = "";
      try
      {
        var data = _supplierService.Create(supplier, out strMessage);
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }
    [HttpPost("Update-Supplier")]
    public async Task<ApiResponse<Supplier>> UpdateSupplỉer([FromBody] Supplier supplier)
    {
      ApiResponse<Supplier> result;
      string strMessage = "";
      try
      {
        var data = _supplierService.Update(supplier, out strMessage);
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Supplier>>> Delete(int id)
    {
      string strMessage = "";
      try
      {
        var data = _supplierService.Delete(id, out strMessage);
        var result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        return BadRequest(new ApiResponse<Supplier>(System.Net.HttpStatusCode.BadRequest, strMessage, null));
      }
    }
  }
}
