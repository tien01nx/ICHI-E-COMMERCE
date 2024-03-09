using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.NlogConfig;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class CustomerController : BaseController<Customer>
  {
    private readonly ICustomerService _customerService;
    public CustomerController(PcsApiContext context, ICustomerService customerService) : base(context)
    {
      _customerService = customerService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Customer>> result;
      string strMessage = "";
      try
      {
        var data = _customerService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>(
             System.Net.HttpStatusCode.OK,
             strMessage,
             data
         );
      }
      catch (Exception ex)
      {
        strMessage = "Có lỗi xảy ra";
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Customer>>> Delete(int id)
    {
      string strMessage = "";
      try
      {
        var data = _customerService.Delete(id, out strMessage);
        var result = new ApiResponse<Customer>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        return BadRequest(new ApiResponse<Customer>(System.Net.HttpStatusCode.BadRequest, strMessage, null));
      }
    }
  }
}
