using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class EmployeeController : ControllerBase
  {
    private IEmployeeService _employeeService;
    public EmployeeController(PcsApiContext context, IEmployeeService employeeService)
    {
      _employeeService = employeeService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Employee>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Employee>> result;
      string strMessage = "";
      try
      {
        var data = _employeeService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);

        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Employee>>(
             System.Net.HttpStatusCode.OK,
             strMessage,
             data);
        return result;
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<Employee>>(ex);
      }
    }

    [HttpPut("Update")]
    public async Task<ActionResult<ApiResponse<Employee>>> Update([FromForm] Employee customer, IFormFile? file)
    {
      ApiResponse<Employee> result;
      string strMessage = "";
      try
      {
        var data = _employeeService.Update(customer, file, out strMessage);
        result = new ApiResponse<Employee>(
          System.Net.HttpStatusCode.OK,
          strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Employee>(ex);
      }
      return result;
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<ApiResponse<Employee>>> Delete(int id)
    {
      string strMessage = "";
      try
      {
        var data = _employeeService.Delete(id, out strMessage);
        var result = new ApiResponse<Employee>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Employee>(ex);
      }
    }

  }
}
