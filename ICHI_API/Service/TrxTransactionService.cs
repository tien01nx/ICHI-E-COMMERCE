using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class TrxTransactionService : ITrxTransactionService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    private readonly IPromotionService _promotionService;
    public TrxTransactionService(IUnitOfWork unitOfWork, IPromotionService promotionService, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
      _promotionService = promotionService;
    }
    public Helpers.PagedResult<TrxTransaction> GetAll(string name, string orderStatus, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.TrxTransactions.OrderByDescending(u => u.OrderDate).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
        }
        if (!string.IsNullOrEmpty(orderStatus))
        {
          query = query.Where(e => e.OrderStatus.Contains(orderStatus));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<TrxTransaction>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }


    public TrxTransactionDTO InsertTxTransaction(TrxTransactionDTO trxTransactionDTO, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        int checkPromotion = trxTransactionDTO.Carts.Where(x => x.Discount > 0).Count();
        // kiểm tra thông tin product trong carts để kiểm tra còn trong chương trình khuyến mãi không
        var promotion = _promotionService.CheckPromotionActive().Select(x => x.ProductId);

        var cartProduct = trxTransactionDTO.Carts.Where(x => x.Discount > 0 && promotion.Contains(x.ProductId)).ToList();
        if (cartProduct.Count == 0 && checkPromotion > 0)
        {
          strMessage = "Mã khuyến mãi đã hết vui lòng thử lại sau!";
          return null;
        }
        // từ userId lấy ra customerId
        // từ userId lấy ra xem là employee hay customer từ bảng UserRole
        TrxTransaction trxTransaction = new TrxTransaction();

        var user = _unitOfWork.UserRole.Get(u => u.UserId == trxTransactionDTO.UserId, "Role");
        if (user == null)
        {
          strMessage = "Không tìm thấy thông tin người dùng";
          return null;
        }
        if (user.Role.RoleName == AppSettings.USER)
        {
          trxTransaction.CustomerId = _unitOfWork.Customer.Get(u => u.UserId == trxTransactionDTO.UserId).Id;
        }
        else
        {
          strMessage = "Không tìm thấy thông tin người dùng";
          return null;
        }

        var cart = _unitOfWork.Cart.GetAll(u => u.UserId == trxTransactionDTO.UserId);
        trxTransaction.FullName = trxTransactionDTO.FullName;
        trxTransaction.PhoneNumber = trxTransactionDTO.PhoneNumber;
        trxTransaction.Address = trxTransactionDTO.Address;
        trxTransaction.OrderDate = DateTime.Now;
        trxTransaction.OrderStatus = AppSettings.StatusOrderPending;
        trxTransaction.PaymentTypes = trxTransactionDTO.PaymentTypes;
        trxTransaction.PaymentStatus = AppSettings.PaymentStatusPending;
        trxTransaction.OrderTotal = trxTransactionDTO.Amount ?? 0;
        _unitOfWork.TrxTransaction.Add(trxTransaction);
        _unitOfWork.Save();

        trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
        trxTransactionDTO.Amount = trxTransaction.OrderTotal;
        // lấy thông tin đơn hàng theo userid từ cart
        foreach (var item in cart)
        {
          TransactionDetail trxTransactionDetail = new TransactionDetail();
          trxTransactionDetail.ProductId = item.ProductId;
          trxTransactionDetail.Total = item.Quantity;
          trxTransactionDetail.Price = item.Price;
          trxTransactionDetail.TrxTransactionId = trxTransaction.Id;
          _unitOfWork.TransactionDetail.Add(trxTransactionDetail);
          _unitOfWork.Save();
        }
        if (checkPromotion > 0)
        {
          // cập nhật discount promtionDetail cho productID
          foreach (var item in cartProduct)
          {
            var productId = _unitOfWork.PromotionDetail.Get(u => u.ProductId == item.ProductId, tracked: true);
            productId.UsedCodesCount += 1;
            _unitOfWork.PromotionDetail.Update(productId);
          }
        }
        // xóa thông tin giỏ hàng theo userid
        _unitOfWork.Cart.RemoveRange(cart);
        _unitOfWork.Save();
        _unitOfWork.Commit();
        return trxTransactionDTO;
      }
      catch (Exception ex)
      {
        _unitOfWork.Rollback();
        strMessage = ex.Message;
        NLogger.log.Error(ex.ToString());
        return null;
      }
    }

    public ShoppingCartVM GetTrxTransactionFindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        ShoppingCartVM cartVM = new ShoppingCartVM();
        cartVM.TrxTransaction = _unitOfWork.TrxTransaction.Get(u => u.Id == id);
        if (cartVM.TrxTransaction == null)
        {
          strMessage = "Không tìm thấy đơn hàng";
          return null;
        }

        cartVM.Customer = _unitOfWork.Customer.Get(u => u.Id == cartVM.TrxTransaction.CustomerId);
        cartVM.TransactionDetail = _unitOfWork.TransactionDetail.GetAll(u => u.TrxTransactionId == id, "Product");
        foreach (var item in cartVM.TransactionDetail)
        {
          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
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

    // lấy thông tin khách hàng theo customerid và email
    public CustomerTransactionDTO GetCustomerTransaction(string userid, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        CustomerTransactionDTO customerTransactionDTO = new CustomerTransactionDTO();
        var customer = _unitOfWork.Customer.Get(u => u.UserId == userid, "User");
        if (customer == null)
        {
          strMessage = "Không tìm thấy thông tin khách hàng";
          return null;
        }
        customerTransactionDTO.Customer = customer;
        customerTransactionDTO.TrxTransactions = _unitOfWork.TrxTransaction.GetAll(u => u.CustomerId == customer.Id).OrderByDescending(u => u.OrderDate).ToList();
        return customerTransactionDTO;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

  }
}