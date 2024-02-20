using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using System.Net.Http.Headers;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;

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
                _context.Products.Update(data);
                await _context.SaveChangesAsync();
                var result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Product>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }
    }

}
