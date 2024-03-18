using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using ICHI_API;
using ICHI_API.Data;

namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        public ProductImagesController(PcsApiContext context) { }

    }
}