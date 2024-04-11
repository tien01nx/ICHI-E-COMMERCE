
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using iText.Html2pdf;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CartService : ICartService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IPromotionService _promotionService;

    public CartService(IUnitOfWork unitOfWork, IPromotionService promotionService, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
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
        var promotionDetail = _promotionService.CheckPromotionActive();
        foreach (var item in data)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
          item.SetCustomer(_unitOfWork.Customer.Get(u => u.UserId == item.UserId));
          item.Discount = promotionDetail.Where(u => u.ProductId == item.ProductId).FirstOrDefault()?.Promotion?.Discount ?? 0;
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
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
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
          item.Discount = promotionDetail.Where(u => u.ProductId == item.ProductId).FirstOrDefault()?.Promotion?.Discount ?? 0;
        }
        return trxTransaction;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public ShoppingCartVM GetShoppingCart(string email, List<Cart> carts, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        ShoppingCartVM cartVM = new ShoppingCartVM();
        cartVM.Cart = CheckCartPromotion(carts, out strMessage);

        // thực hiện lấy thông tin từ email và từ đó lấy ra role => bảng employee hay customer
        var roles = _unitOfWork.UserRole.Get(u => u.UserId == email, "User,Role");
        if (roles.Role.RoleName == AppSettings.USER)
        {
          cartVM.Customer = _unitOfWork.Customer.Get(u => u.UserId == email);
          cartVM.TrxTransaction = new TrxTransaction();
          //cartVM.TrxTransaction.Address = cartVM.Cu;
          // nếu address trong customer null thì hiện vui lòng nhập địa chỉ
          cartVM.TrxTransaction.Address = cartVM?.Customer?.Address ?? "Vui lòng nhập địa chỉ";
          cartVM.TrxTransaction.PhoneNumber = cartVM.Customer?.PhoneNumber;
          cartVM.TrxTransaction.FullName = cartVM?.Customer?.FullName;
        }
        else
        {
          var data = _unitOfWork.Employee.Get(u => u.UserId == email);
          string Address = data.Address;
          string PhoneNumber = data.PhoneNumber;
          string FullName = data.FullName;
          cartVM.TrxTransaction = new TrxTransaction();
          cartVM.TrxTransaction.Address = Address ?? "Vui lòng nhập địa chỉ";
          cartVM.TrxTransaction.PhoneNumber = PhoneNumber;
          cartVM.TrxTransaction.FullName = FullName;
        }

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
        var product = _unitOfWork.Product.Get(u => u.Id == cart.ProductId);
        if (existingCartItem != null)
        {
          // nếu quantity > quantity trong product thì thông báo số lượng sản phẩm không đủ và set existingCartItem.Quantity = product.Quantity
          if (existingCartItem.Quantity + cart.Quantity > product.Quantity)
          {
            strMessage = "Số lượng sản phẩm không đủ";
            existingCartItem.Quantity = product.Quantity;
            _unitOfWork.Cart.Update(existingCartItem);
            return cart;
          }
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

        strMessage = "Có lỗi xảy ra vui lòng thử lại sau!";
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
        var product = _unitOfWork.Product.Get(u => u.Id == cart.ProductId);

        if (data != null)
        {
          if (cart.Quantity > product.Quantity)
          {
            strMessage = "Số lượng sản phẩm không đủ/ hết hàng";
            if (product.Quantity == 0)
            {
              data.Quantity = 1;
            }
            else
            {
              data.Quantity = product.Quantity;
            }
            _unitOfWork.Cart.Update(data);
            _unitOfWork.Save();
            return data;
          }
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


    static void ConvertHtmlToPdf(string htmlFile, string pdfFile)
    {
      using (FileStream htmlSource = File.Open("http://localhost:4200/invoice", FileMode.Open))
      using (FileStream pdfDest = File.Open(pdfFile, FileMode.Create))
      {
        HtmlConverter.ConvertToPdf(htmlSource, pdfDest);
      }
    }
  }
}
