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
    public class SupplierController : BaseController<Supplier>
    {
        public SupplierController(PcsApiContext context) : base(context) { }

        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Supplier>>>> FindAllPaged(PaginationParams paginationParams)
        {
            try
            {
                IQueryable<Supplier> query = _context.Suppliers.Where(x => !x.IsDeleted);

                if (!string.IsNullOrWhiteSpace(paginationParams.Search))
                {
                    query = query.Where(p => p.SupplierName.Contains(paginationParams.Search));
                }

                var direction = paginationParams.SortDirection.ToLower() == "asc" ? "" : " descending";
                query = query.OrderBy($"{paginationParams.SortBy}{direction}");

                var data = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();
                var totalRecords = await query.CountAsync();

                var responseData = new PagedList<Supplier>(data, totalRecords, paginationParams.PageNumber, paginationParams.PageSize);
                var result = new ApiResponse<IEnumerable<Supplier>>(System.Net.HttpStatusCode.OK, "", responseData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<Supplier>>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }
    }
}
