
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CartService : ICartService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;

    public CartService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
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
          strMessage = "Không tìm thấy sản phẩm trong giỏ hàng";
          return null;
        }

        strMessage = "Xóa sản phẩm khỏi giỏ hàng thành công";
        _unitOfWork.Save();
        return cart;
      }
      catch (Exception ex)
      {
        strMessage = ex.Message;
        NLogger.log.Error(ex.ToString());
        return null;
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
        foreach (var item in data)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
          item.SetCustomer(_unitOfWork.Customer.Get(u => u.UserId == item.UserId));
        }


        return data;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public ShoppingCartVM GetShoppingCart(string email, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        ShoppingCartVM cartVM = new ShoppingCartVM();
        cartVM.Cart = _unitOfWork.Cart.GetAll(u => u.UserId == email, "User,Product");
        foreach (var item in cartVM.Cart)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
        }
        cartVM.Customer = _unitOfWork.Customer.Get(u => u.UserId == email);
        cartVM.TrxTransaction = new TrxTransaction();
        //cartVM.TrxTransaction.Address = cartVM.Cu;
        // nếu address trong customer null thì hiện vui lòng nhập địa chỉ
        cartVM.TrxTransaction.Address = cartVM.Customer.Address ?? "Vui lòng nhập địa chỉ";
        cartVM.TrxTransaction.PhoneNumber = cartVM.Customer.PhoneNumber;
        cartVM.TrxTransaction.FullName = cartVM.Customer.FullName;
        return cartVM;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }



    public Cart InsertCard(Cart cart, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var existingCartItem = _unitOfWork.Cart.Get(u => u.UserId == cart.UserId && u.ProductId == cart.ProductId);

        if (existingCartItem != null)
        {
          existingCartItem.Quantity += cart.Quantity;
          _unitOfWork.Cart.Update(existingCartItem);
        }
        else
        {
          _unitOfWork.Cart.Add(cart);
        }

        strMessage = "Thêm vào giỏ hàng thành công";
        _unitOfWork.Save();
        return cart;
      }
      catch (Exception ex)
      {
        strMessage = ex.Message;
        NLogger.log.Error(ex.ToString());
        return null;
      }
    }

    // update số lượng sản phẩm trong giỏ hàng
    public Cart UpdateCart(Cart cart, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Cart.Get(u => u.UserId == cart.UserId && u.ProductId == cart.ProductId && u.Id == cart.Id);
        if (data != null)
        {
          data.Quantity = cart.Quantity;
          _unitOfWork.Cart.Update(data);
          strMessage = "Cập nhật giỏ hàng thành công";
          _unitOfWork.Save();
        }
        else
        {
          strMessage = "Không tìm thấy sản phẩm trong giỏ hàng";
          return null;
        }
        return data;
      }
      catch (Exception ex)
      {
        strMessage = ex.Message;
        NLogger.log.Error(ex.ToString());
        return null;
      }
    }
  }
}
