using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using ICHI_CORE.Helpers;
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
        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryProduct>>>> FindAllPaged([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                IQueryable<CategoryProduct> query = _context.CategoryProducts.Where(x => !x.IsDeleted);

                // Áp dụng tìm kiếm
                if (!string.IsNullOrWhiteSpace(paginationParams.Search))
                {
                    query = query.Where(p => p.CategoryName.Contains(paginationParams.Search));
                }

                // Áp dụng sắp xếp
                var direction = paginationParams.SortDirection.ToLower() == "asc" ? "" : " descending";
                query = query.OrderBy($"{paginationParams.SortBy}{direction}");

                // Áp dụng phân trang
                var data = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();
                var totalRecords = await query.CountAsync();

                var responseData = new PagedList<CategoryProduct>(data, totalRecords, paginationParams.PageNumber, paginationParams.PageSize);
                var result = new ApiResponse<IEnumerable<CategoryProduct>>(System.Net.HttpStatusCode.OK, "", responseData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryProduct>>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
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
