namespace ICHI_API.Controllers.MasterController
{
  using ICHI_API.Data;
  using ICHI_API.Model;
  using ICHI_API.Service.IService;
  using ICHI_CORE.Domain.MasterModel;
  using ICHI_CORE.Model;
  using Microsoft.AspNetCore.Mvc;
  using System.Collections.Generic;

  [ApiController]
  [Route("api/[controller]")]
  public class CartController : Controller
  {
    private readonly ICartService _cartService;

    public CartController(PcsApiContext context, ICartService cartService, IConfiguration configuration = null)
    {
      _cartService = cartService;
    }

    [HttpPost("AddtoCart")]
    public async Task<ApiResponse<Cart>> AddtoCart([FromBody] Cart cart)
    {
      ApiResponse<Cart> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.InsertCard(cart, out strMessage);
        return new ApiResponse<Cart>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {

        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Cart>(ex);
      }
    }

    [HttpGet("GetCarts")]
    public async Task<ApiResponse<IEnumerable<Cart>>> GetCarts(string email)
    {
      ApiResponse<IEnumerable<Cart>> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.GetCarts(email, out strMessage);
        return new ApiResponse<IEnumerable<Cart>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<IEnumerable<Cart>>(ex);
      }
    }



    [HttpDelete("DeleteCart")]
    public async Task<ApiResponse<Cart>> DeleteCart([FromBody] Cart cart)
    {
      ApiResponse<Cart> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.DeleteCart(cart, out strMessage);
        return new ApiResponse<Cart>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Cart>(ex);
      }
    }

    [HttpPost("GetShoppingCart")]
    public async Task<ApiResponse<ShoppingCartVM>> GetShoppingCart(CartProductDTO cart)
    {
      ApiResponse<ShoppingCartVM> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.GetShoppingCart(cart.email, cart.carts, out strMessage);
        return new ApiResponse<ShoppingCartVM>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<ShoppingCartVM>(ex);

      }
    }

    [HttpPost("CheckCartPromotion")]
    public async Task<ApiResponse<IEnumerable<Cart>>> CheckCartPromotion([FromBody] List<Cart> carts)
    {
      ApiResponse<IEnumerable<Cart>> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.CheckCartPromotion(carts, out strMessage);
        return new ApiResponse<IEnumerable<Cart>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        //strMessage = "Có lỗi xảy ra";
        //NLogger.log.Error(ex.ToString());
        //result = new ApiResponse<IEnumerable<Cart>>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<IEnumerable<Cart>>(ex);
      }
    }

    [HttpGet("CheckTransactionPromotion")]
    public async Task<ApiResponse<IEnumerable<TransactionDetail>>> CheckCartPromotion(int transactionId)
    {
      ApiResponse<IEnumerable<TransactionDetail>> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.CheckCartPromotion(transactionId, out strMessage);
        return new ApiResponse<IEnumerable<TransactionDetail>>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<IEnumerable<TransactionDetail>>(ex);
      }
    }

    [HttpPut("UpdateCart")]
    public async Task<ApiResponse<Cart>> UpdateCart([FromBody] Cart cart)
    {
      ApiResponse<Cart> result;
      string strMessage = string.Empty;
      try
      {
        var data = _cartService.UpdateCart(cart, out strMessage);
        return new ApiResponse<Cart>(System.Net.HttpStatusCode.OK, strMessage, data);
      }
      catch (Exception ex)
      {
        var handler = new GlobalExceptionHandler();
        return handler.HandleException<Cart>(ex);
      }

    }

  }
}
