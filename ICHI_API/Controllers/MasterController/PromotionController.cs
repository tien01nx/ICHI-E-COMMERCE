using ICHI_API.Data;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class PromotionController : ControllerBase
  {
    private readonly IPromotionService _PromotionService;
    public PromotionController(PcsApiContext context, IPromotionService PromotionService)
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
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ICHI_API.Helpers.PagedResult<Promotion>>(ex);
      }
      return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PromotionDTO>>> FindById(int id)
    {
      ApiResponse<PromotionDTO> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.FindById(id, out strMessage);
        result = new ApiResponse<PromotionDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<PromotionDTO>(ex);
      }
      return result;
    }

    [HttpPost("Create")]
    public async Task<ApiResponse<PromotionDTO>> CreateSupplỉer([FromBody] PromotionDTO Promotion)
    {
      ApiResponse<PromotionDTO> result;
      string strMessage = "";

      if (Promotion.PromotionDetails.Count() == 0)
      {
        strMessage = "Yêu cầu phải có sản phẩm";
        return new ApiResponse<PromotionDTO>(System.Net.HttpStatusCode.BadRequest, strMessage, null);
      }
      try
      {
        var data = _PromotionService.Create(Promotion, out strMessage);
        result = new ApiResponse<PromotionDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<PromotionDTO>(ex);
      }
      return result;
    }
    [HttpPut("Update")]
    public async Task<ApiResponse<PromotionDTO>> UpdateSupplỉer([FromBody] PromotionDTO Promotion)
    {
      ApiResponse<PromotionDTO> result;
      string strMessage = "";
      try
      {
        var data = _PromotionService.Update(Promotion, out strMessage);
        result = new ApiResponse<PromotionDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<PromotionDTO>(ex);
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
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Promotion>(ex);
      }
    }
  }
}
