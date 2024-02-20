using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;

namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderHeadersController : BaseController<Product>
    {
        public OrderHeadersController(PcsApiContext context) : base(context) { }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<OrderHeaders>>> Delete(int id)
        {
            try
            {
                var data = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);
                data.IsDeleted = true;
                data.UpdateDatetime = DateTime.Now;
                data.UpdateUserId = "Admin";
                _context.OrderHeaders.Update(data);
                await _context.SaveChangesAsync();
                var result = new ApiResponse<OrderHeaders>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OrderHeaders>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }

    }
}
