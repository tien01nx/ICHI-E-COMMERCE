using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_CORE.NlogConfig;
namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryProductController : BaseController<CategoryProduct>
    {
        public CategoryProductController(PcsApiContext context) : base(context) { }

        //public int PageNumber { get; set; } = AppSettings.PageNumber;
        //public int PageSize { get; set; } = AppSettings.PageSize;
        //public string Search { get; set; } = string.Empty;
        //public string SortBy { get; set; } = AppSettings.SortBy;
        //public string SortDirection { get; set; } = AppSettings.SortDirection;
        //[HttpGet("FindAllPaged")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<CategoryProduct>>>> FindAllPaged(
        //                [FromQuery(Name = "search")] string name = "",
        //                [FromQuery(Name = "page-size")] int pageSize = 10,
        //                [FromQuery(Name = "page-number")] int pageNumber = 1,
        //                [FromQuery(Name = "sort-direction")] string sortDir = "desc",
        //                [FromQuery(Name = "sort-by")] string sortBy = "Id")
        //{
        //    try
        //    {
        //        IQueryable<CategoryProduct> query = _context.CategoryProducts.Where(x => !x.IsDeleted);

        //        // Áp dụng tìm kiếm
        //        if (!string.IsNullOrWhiteSpace(name))
        //        {
        //            query = query.Where(p => p.CategoryName.Contains(name));
        //        }

        //        // Áp dụng sắp xếp
        //        var direction = sortDir.ToLower() == "asc" ? "" : " descending";
        //        query = query.OrderBy($"{sortDir}{direction}");

        //        // Áp dụng phân trang
        //        var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //        var totalRecords = await query.CountAsync();

        //        var responseData = new PagedList<CategoryProduct>(data, totalRecords, pageNumber, pageSize);
        //        var result = new ApiResponse<IEnumerable<CategoryProduct>>(System.Net.HttpStatusCode.OK, "", responseData);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse<IEnumerable<CategoryProduct>>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
        //    }
        //}


        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<CategoryProduct>>>> GetAll(
                  [FromQuery(Name = "search")] string name = "",
                  [FromQuery(Name = "page-size")] int pageSize = 10,
                  [FromQuery(Name = "page-number")] int pageNumber = 1,
                  [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                  [FromQuery(Name = "sort-by")] string sortBy = "Id")
        {
            ApiResponse<ICHI_API.Helpers.PagedResult<CategoryProduct>> result;
            try
            {
                var query = _context.CategoryProducts.AsQueryable().Where(u => u.IsDeleted == false);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.CategoryName.Contains(name));
                }

                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);

                var pagedResult = await ICHI_API.Helpers.PagedResult<CategoryProduct>.CreatePagedResultAsync(query, pageNumber, pageSize);

                result = new ApiResponse<ICHI_API.Helpers.PagedResult<CategoryProduct>>(
                     System.Net.HttpStatusCode.OK,
                     "Retrieved successfully",
                     pagedResult
                 );
            }
            catch (Exception ex)
            {
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<CategoryProduct>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
            }
            return result;
        }

        // viết API get theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryProduct>>> FindById(int id)
        {
            try
            {
                var data = await _context.CategoryProducts.FirstOrDefaultAsync(x => x.Id == id);
                var result = new ApiResponse<CategoryProduct>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryProduct>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }

        // viết API delete theo id
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryProduct>>> Delete(int id)
        {
            try
            {
                var data = await _context.CategoryProducts.FirstOrDefaultAsync(x => x.Id == id);
                data.IsDeleted = true;
                data.UpdateDatetime = DateTime.Now;
                data.UpdateUserId = "Admin";
                _context.CategoryProducts.Update(data);
                await _context.SaveChangesAsync();
                var result = new ApiResponse<CategoryProduct>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryProduct>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }
    }
}
