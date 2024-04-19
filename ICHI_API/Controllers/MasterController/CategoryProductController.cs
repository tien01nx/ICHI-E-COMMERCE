using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoryProductController : ControllerBase
  {
    private readonly ICategoryProductService _categoryproductService;

    public CategoryProductController(PcsApiContext context, ICategoryProductService categoryProductService)
    {
      _categoryproductService = categoryProductService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Category>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Category>> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Category>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             data
         );
        return result;
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<Category>>(ex);
      }
    }

    [HttpGet("FindAll")]
    public async Task<ActionResult<ApiResponse<List<Category>>>> FindAll()
    {
      ApiResponse<List<Category>> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.FindAll();
        result = new ApiResponse<List<Category>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        //NLogger.log.Error(ex.ToString());
        //strMessage = "Có lỗi xảy ra";
        //result = new ApiResponse<List<Category>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<List<Category>>(ex);
      }
      return result;

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> FindById(int id)
    {
      ApiResponse<Category> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.FindById(id, out strMessage);
        result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Category>(ex);
      }
      return result;
    }

    [HttpGet("GetCategoryByProduct")]
    public async Task<ActionResult<ApiResponse<List<Category>>>> GetCategoryByProduct(string categoryname)
    {
      ApiResponse<List<Category>> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.GetCategoriesByParentID(categoryname, out strMessage);
        result = new ApiResponse<List<Category>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<List<Category>>(ex);
      }
      return result;
    }

    [HttpPost("Create")]
    public async Task<ApiResponse<Category>> Create([FromBody] Category category)
    {
      ApiResponse<Category> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.Create(category, out strMessage);
        result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Category>(ex);
      }
      return result;
    }

    [HttpPost("GetCategoryLevels")]

    public async Task<ApiResponse<List<Category>>> GetCategoryLevels([FromBody] Category category)
    {
      ApiResponse<List<Category>> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.GetCategoryLevels(category);
        result = new ApiResponse<List<Category>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<List<Category>>(ex);
      }
      return result;
    }


    [HttpPut("Update")]
    public async Task<ApiResponse<Category>> UpdateSupplỉer([FromBody] Category supplier)
    {
      ApiResponse<Category> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.Update(supplier, out strMessage);
        result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Category>(ex);
      }
      return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> Delete(int id)
    {
      string strMessage = "";
      try
      {
        var data = _categoryproductService.Delete(id, out strMessage);
        var result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Category>(ex);
      }
    }

    [HttpGet("Test")]
    public async Task<ApiResponse<DataTable>> Test()
    {
      ApiResponse<DataTable> result;
      string strMessage = "";
      try
      {
        var data = _categoryproductService.GetData();
        result = new ApiResponse<DataTable>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<DataTable>(ex);
      }
      return result;
    }
  }
}
