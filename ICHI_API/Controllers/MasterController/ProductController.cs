using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using System.Net.Http.Headers;

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
     
    }
        
}
