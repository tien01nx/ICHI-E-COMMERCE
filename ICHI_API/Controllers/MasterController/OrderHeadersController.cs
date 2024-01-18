using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderHeadersController :  BaseController<Product>
    {
        public OrderHeadersController(PcsApiContext context) : base(context) { }

    }
}
