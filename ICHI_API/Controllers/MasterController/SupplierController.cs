using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : BaseController<Supplier>
    {
        public SupplierController(PcsApiContext context) : base(context) { }



        //[HttpGet("FindAllPaged")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<Supplier>>>> FindAllPaged([FromQuery] PaginationParams paginationParams)
        //{
        //    try
        //    {
        //        IQueryable<Supplier> query = _context.Suppliers.Where(x => !x.IsDeleted);

        //        if (!string.IsNullOrWhiteSpace(paginationParams.Search))
        //        {
        //            query = query.Where(p => p.SupplierName.Contains(paginationParams.Search));
        //        }

        //        var direction = paginationParams.SortDirection.ToLower() == "asc" ? "" : " descending";
        //        query = query.OrderBy($"{paginationParams.SortBy}{direction}");

        //        var data = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();
        //        var totalRecords = await query.CountAsync();

        //        var responseData = new PagedList<Supplier>(data, totalRecords, paginationParams.PageNumber, paginationParams.PageSize);
        //        var result = new ApiResponse<IEnumerable<Supplier>>(System.Net.HttpStatusCode.OK, "", responseData);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse<IEnumerable<Supplier>>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
        //    }
        //}

        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<PagedResult<Supplier>>>> GetAll(
                        [FromQuery(Name = "search")] string name = "",
                        [FromQuery(Name = "page-size")] int pageSize = 10,
                        [FromQuery(Name = "page-number")] int pageNumber = 1,
                        [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                        [FromQuery(Name = "sort-by")] string sortBy = "Id")
        {
            var query = _context.Suppliers.AsQueryable().Where(u => u.IsDeleted == false);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.SupplierName.Contains(name));
            }

            // Sắp xếp theo cột và hướng sắp xếp
            var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
            query = query.OrderBy(orderBy);

            // Sử dụng phương thức tạo tĩnh để tạo PagedResult
            var pagedResult = await ICHI_API.Helpers.PagedResult<Supplier>.CreatePagedResultAsync(query, pageNumber, pageSize);

            var response = new ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>(
                  System.Net.HttpStatusCode.OK,
                  "Retrieved successfully",
                  pagedResult
              );

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Supplier>>> Delete(int id)
        {
            try
            {
                var data = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
                data.IsDeleted = true;
                data.UpdateDatetime = DateTime.Now;
                data.UpdateUserId = "Admin";
                _context.Suppliers.Update(data);
                await _context.SaveChangesAsync();
                var result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Supplier>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }
    }
}
