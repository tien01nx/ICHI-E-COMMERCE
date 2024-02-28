using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using System.Net.Http.Headers;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController<Product>
    {
        public ProductController(PcsApiContext context) : base(context) { }

        [HttpPost]
        [Route("UploadImage")]
        public IActionResult UploadImage([FromBody] Product request)
        {
            return Ok(new { ProductId = request.Id, DisplayValue = request.DisplayValue });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> Delete(int id)
        {
            try
            {
                var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                data.IsDeleted = true;
                data.UpdateDatetime = DateTime.Now;
                data.UpdateUserId = "Admin";
                await Update(data);
                var result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Product>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }



        [HttpGet("FindAllPaged")]
        public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Product>>>> GetAll(
                [FromQuery(Name = "search")] string name = "",
                [FromQuery(Name = "page-size")] int pageSize = 10,
                [FromQuery(Name = "page-number")] int pageNumber = 1,
                [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                [FromQuery(Name = "sort-by")] string sortBy = "Id")
        {
            ApiResponse<ICHI_API.Helpers.PagedResult<Product>> result;
            try
            {
                var query = _context.Products.Include(u => u.CategoryProduct).AsQueryable().Where(u => u.IsDeleted == false);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.Description.Contains(name));
                }

                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);
                var pagedResult = await ICHI_API.Helpers.PagedResult<Product>.CreatePagedResultAsync(query, pageNumber, pageSize);

                result = new ApiResponse<ICHI_API.Helpers.PagedResult<Product>>(
                     System.Net.HttpStatusCode.OK,
                     "Retrieved successfully",
                     pagedResult
                 );
            }
            catch (Exception ex)
            {
                result = new ApiResponse<ICHI_API.Helpers.PagedResult<Product>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
            }
            return result;
        }



        // API v1/Create
        [HttpPost("Create-Product")]
        public async Task<ApiResponse<Product>> CreateSupplỉer(Product product)
        {
            ApiResponse<Product> result;
            try
            {
                // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
                var checkProduct = await _context.Products.FirstOrDefaultAsync(x => x.Description == product.Description);
                if (checkProduct != null)
                {
                    result = new ApiResponse<Product>(System.Net.HttpStatusCode.Forbidden, "Product code already exists", null);
                    return result;
                }

                product.CreateUserId = "Admin";
                product.UpdateUserId = "Admin";

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                var data = await GetByKeys(product);
                result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "Created successfully", data);
            }
            catch (Exception ex)
            {
                result = new ApiResponse<Product>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
            }
            return result;
        }
    }

}
