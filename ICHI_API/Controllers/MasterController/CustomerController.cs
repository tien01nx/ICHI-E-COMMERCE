using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class CustomerController : ControllerBase
  {
    private readonly ICustomerService _customerService;
    public CustomerController(PcsApiContext context, ICustomerService customerService)
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
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<Customer>>(ex);
      }
      return result;
    }

    // cập nhật khách hàng 
    [HttpPut("Update")]
    public async Task<ActionResult<ApiResponse<Customer>>> Update([FromForm] Customer customer, IFormFile? file)
    {
      ApiResponse<Customer> result;
      string strMessage = "";
      try
      {
        var data = _customerService.Update(customer, file, out strMessage);
        result = new ApiResponse<Customer>(
          System.Net.HttpStatusCode.OK,
          strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Customer>(ex);
      }
      return result;
    }

    [HttpDelete("Delete")]
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
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Customer>(ex);

      }
    }
  }
}
