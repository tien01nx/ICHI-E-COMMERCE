
using DevExpress.CodeParser;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CartService : ICartService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IPromotionService _promotionService;
    private readonly ITrxTransactionService _trxTransactionService;

    public CartService(IUnitOfWork unitOfWork, IPromotionService promotionService, ITrxTransactionService trxTransactionService, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
      _trxTransactionService = trxTransactionService;
      _promotionService = promotionService;
    }

    public Cart DeleteCart(Cart cart, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var existingCartItem = _unitOfWork.Cart.Get(u => u.UserId == cart.UserId && u.ProductId == cart.ProductId);

        if (existingCartItem != null)
        {
          _unitOfWork.Cart.Remove(existingCartItem);
        }
        else
        {
          throw new BadRequestException(Constants.PRODUCTCCARTNOTFOUND);
        }

        strMessage = Constants.DELETECARTSUCCESS;
        _unitOfWork.Save();
        return cart;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IEnumerable<Cart> GetCarts(string email, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Cart.GetAll(u => u.UserId == email, "User,Product");

        if (data == null)
        {
          return null;
        }
        var promotionDetail = _promotionService.CheckPromotionActive();
        foreach (var item in data)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
          item.SetCustomer(_unitOfWork.Customer.Get(u => u.UserId == item.UserId));
          item.Discount = promotionDetail.Where(u => u.ProductId == item.ProductId).FirstOrDefault()?.Promotion?.Discount ?? 0;
        }

        return data;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IEnumerable<Cart> CheckCartPromotion(List<Cart> carts, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var promotionDetail = _promotionService.CheckPromotionActive();
        foreach (var item in carts)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
          item.SetCustomer(_unitOfWork.Customer.Get(u => u.UserId == item.UserId));
          item.Discount = promotionDetail.Where(u => u.ProductId == item.ProductId).FirstOrDefault()?.Promotion?.Discount ?? 0;
        }
        return carts;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IEnumerable<TransactionDetail> CheckCartPromotion(int trxTransactionId, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var trxTransaction = _unitOfWork.TransactionDetail.GetAll(u => u.TrxTransactionId == trxTransactionId, "TrxTransaction,Product");
        var promotionDetail = _promotionService.GetPromotionDetailActive(trxTransaction.FirstOrDefault().TrxTransaction.OrderDate);

        foreach (var item in trxTransaction)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
          item.Discount = promotionDetail.Where(u => u.ProductId == item.ProductId && u.UsedCodesCount < u.Quantity).FirstOrDefault()?.Promotion?.Discount ?? 0;
        }
        return trxTransaction;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public ShoppingCartVM GetShoppingCart(string email, List<Cart> carts, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        ShoppingCartVM cartVM = new ShoppingCartVM();
        var customer = _unitOfWork.Customer.Get(u => u.UserId == email);
        if (customer == null)
        {
          throw new BadRequestException(Constants.CUSTOMERNOTFOUND);
        }
        cartVM.Cart = CheckCartPromotion(carts, out strMessage);
        cartVM.Customer = customer;
        cartVM.TrxTransaction = new TrxTransaction();
        cartVM.TrxTransaction.Address = cartVM?.Customer?.Address ?? "Vui lòng nhập địa chỉ";
        cartVM.TrxTransaction.PhoneNumber = cartVM.Customer?.PhoneNumber ?? "Bắt buộc";
        cartVM.TrxTransaction.FullName = cartVM?.Customer?.FullName ?? "Bắt buộc";
        return cartVM;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Cart Insert(Cart cart, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var existCart = _unitOfWork.Cart.Get(u => u.UserId == cart.UserId && u.ProductId == cart.ProductId, "Product");
        if (existCart != null)
        {
          // Nếu số lượng sản phẩm trong giỏ hàng lớn hơn product.quantity
          // thì giảm số lượng sản phẩm trong giỏ hàng xuống còn product.quantity
          if (cart.Quantity + existCart.Quantity > existCart.Product.Quantity)
          {
            strMessage = Constants.PRODUCTNOTENOUGHCART;
            throw  new BadRequestException(Constants.PRODUCTNOTENOUGHOUT);
          }
          else
          {
            existCart.Quantity += cart.Quantity;
          }
          _unitOfWork.Cart.Update(existCart);
        }
        else
        {
          _unitOfWork.Cart.Add(cart);
        }
        strMessage = Constants.ADDCARTSUCCESS;
        _unitOfWork.Save();
        return existCart ?? cart;
      }
      catch (Exception)
      {
        throw;
      }
    }

    // update số lượng sản phẩm trong giỏ hàng
    public Cart Update(Cart cart, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Cart.Get(u => u.UserId == cart.UserId && u.ProductId == cart.ProductId && u.Id == cart.Id, "Product");
        if (data != null)
        {
          if (cart.Quantity > data.Product.Quantity)
          {
            strMessage = Constants.PRODUCTNOTENOUGHOUT;
            return data;
          }
          data.Quantity = cart.Quantity;
          _unitOfWork.Cart.Update(data);
          strMessage = Constants.UPDATECARTSUCCESS;
          _unitOfWork.Save();
        }
        else
        {
          throw new BadRequestException(Constants.PRODUCTCCARTNOTFOUND);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
