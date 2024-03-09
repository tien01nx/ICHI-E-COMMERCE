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
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoryProductController : BaseController<Category>
  {
    public CategoryProductController(PcsApiContext context) : base(context) { }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Category>>>> GetAll(
              [FromQuery(Name = "search")] string name = "",
              [FromQuery(Name = "page-size")] int pageSize = 10,
              [FromQuery(Name = "page-number")] int pageNumber = 1,
              [FromQuery(Name = "sort-direction")] string sortDir = "desc",
              [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Category>> result;
      try
      {
        var query = _context.Categories.AsQueryable().Where(u => u.IsDeleted == false);

        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.CategoryName.Contains(name));
        }

        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);

        var pagedResult = ICHI_API.Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);

        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Category>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             pagedResult
         );
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Category>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    // viết API get theo id
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> FindById(int id)
    {
      try
      {
        var data = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        var result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, "", data);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(new ApiResponse<Category>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
      }
    }

    // viết API delete theo id
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> Delete(int id)
    {
      try
      {
        var data = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        data.IsDeleted = true;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        _context.Categories.Update(data);
        await _context.SaveChangesAsync();
        var result = new ApiResponse<Category>(System.Net.HttpStatusCode.OK, "", data);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(new ApiResponse<Category>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
      }
    }
  }
}
