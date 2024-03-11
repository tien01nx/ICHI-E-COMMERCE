using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_API;
using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.NlogConfig;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class PromotionController : BaseController<Promotion>
  {
    private readonly IPromotionService _PromotionService;
    public PromotionController(PcsApiContext context, IPromotionService PromotionService) : base(context)
    {
      _PromotionService = PromotionService;
    }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Promotion>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Promotion>> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.GetAll(name, pageSize, pageNumber, sortDir, sortBy, out strMessage);
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Promotion>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             data
         );

      }
      catch (Exception ex)
      {
        strMessage = "Có lỗi xảy ra";
        NLogger.log.Error(ex.ToString());
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Promotion>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Promotion>>> FindById(int id)
    {
      ApiResponse<Promotion> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.FindById(id, out strMessage);
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }

    [HttpPost("Create-Promotion")]
    public async Task<ApiResponse<Promotion>> CreateSupplỉer([FromBody] Promotion Promotion)
    {
      ApiResponse<Promotion> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.Create(Promotion, out strMessage);
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
      }
      return result;
    }
    [HttpPost("Update-Promotion")]
    public async Task<ApiResponse<Promotion>> UpdateSupplỉer([FromBody] Promotion Promotion)
    {
      ApiResponse<Promotion> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.Update(Promotion, out strMessage);
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Promotion>>> Delete(int id)
    {
      string strMessage = "";
      try
      {
        var data = _PromotionService.Delete(id, out strMessage);
        var result = new ApiResponse<Promotion>(System.Net.HttpStatusCode.OK, strMessage, null);
        return Ok(result);
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Có lỗi xảy ra";
        return BadRequest(new ApiResponse<Promotion>(System.Net.HttpStatusCode.BadRequest, strMessage, null));
      }
    }
  }
}
