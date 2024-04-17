using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using System.Linq.Dynamic.Core;
using static ICHI_API.Helpers.Constants;


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
        throw;
      }
    }
    public TrxTransactionDTO Insert(TrxTransactionDTO trxTransactionDTO, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        //trxTransactionDTO.CustomerId = trxTransactionDTO.Carts.FirstOrDefault().UserId;
        int checkPromotion = trxTransactionDTO.Carts.Where(x => x.Discount > 0).Count();
        // kiểm tra thông tin product trong carts để kiểm tra còn trong chương trình khuyến mãi không
        var promotion = _promotionService.CheckPromotionActive().Select(x => x.ProductId);

        var cartProduct = trxTransactionDTO.Carts.Where(x => x.Discount > 0 && promotion.Contains(x.ProductId)).ToList();
        if (cartProduct.Count == 0 && checkPromotion > 0)
        {
          //strMessage = TRXTRANSACTIONPROMTION;
          //return null;
          throw new BadRequestException(TRXTRANSACTIONPROMTION);
        }
        // từ userId lấy ra customerId
        // từ userId lấy ra xem là employee hay customer từ bảng UserRole
        TrxTransaction trxTransaction = new TrxTransaction();

        var user = _unitOfWork.UserRole.Get(u => u.UserId == trxTransactionDTO.CustomerId, "Role");
        if (user == null)
        {
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDUSER);
        }
        if (user.Role.RoleName == AppSettings.USER)
        {
          var data3 = _unitOfWork.Customer.Get(u => u.UserId == trxTransactionDTO.CustomerId);
          trxTransaction.CustomerId = _unitOfWork.Customer.Get(u => u.UserId == trxTransactionDTO.CustomerId).Id;
        }
        else
        {
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDUSER);
        }

        //var cart = _unitOfWork.Cart.GetAll(u => u.UserId == trxTransactionDTO.CustomerId);
        trxTransaction.FullName = trxTransactionDTO?.FullName;
        trxTransaction.PhoneNumber = trxTransactionDTO?.PhoneNumber;
        trxTransaction.Address = trxTransactionDTO?.Address;
        trxTransaction.OrderDate = DateTime.Now;
        trxTransaction.OrderStatus = trxTransactionDTO.OrderStatus ?? "PENDING";
        trxTransaction.PaymentTypes = trxTransactionDTO.PaymentTypes;
        // nếu PaymentTypes = CASH thì trạng thái thanh toán là đã thanh toán
        if (trxTransaction.PaymentTypes == AppSettings.Cash)
        {
          trxTransaction.PaymentStatus = AppSettings.PaymentStatusApproved;
        }
        else
        {
          trxTransaction.PaymentStatus = AppSettings.PaymentStatusPending;
        }
        trxTransaction.OrderTotal = trxTransactionDTO.Amount ?? 0;
        _unitOfWork.TrxTransaction.Add(trxTransaction);
        _unitOfWork.Save();

        trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
        trxTransactionDTO.Amount = trxTransaction.OrderTotal;
        // lấy thông tin đơn hàng theo userid từ cart
        foreach (var item in trxTransactionDTO.Carts)
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
        // xóa thông tin giỏ hàng theo list CarrtId
        var listCartId = trxTransactionDTO.Carts.Select(x => x.Id).ToList();
        _unitOfWork.Cart.RemoveRange(_unitOfWork.Cart.GetAll(u => listCartId.Contains(u.Id)));
        _unitOfWork.Save();
        _unitOfWork.Commit();
        return trxTransactionDTO;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ShoppingCartVM Update(UpdateTrxTransaction model, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        var data = _unitOfWork.TrxTransaction.Get(u => u.Id == model.TransactionId);
        if (data == null)
        {
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDORDER);
        }
        if (model.OrderStatus == AppSettings.StatusOrderPending)
        {
          data.OrderDate = DateTime.Now;
        }
        if (model.OrderStatus == AppSettings.StatusOrderDelivered)
        {
          data.OnholDate = DateTime.Now;
        }

        if (model.OrderStatus == AppSettings.StatusOrderWaitingForPickup || (model.OrderStatus == AppSettings.StatusOrderWaitingForDelivery))
        {
          data.WaitingForPickupDate = DateTime.Now;
          data.WaitingForDeliveryDate = DateTime.Now;
        }
        if (model.OrderStatus == AppSettings.StatusOrderDelivered)
        {
          data.DeliveredDate = DateTime.Now;
        }
        if (model.OrderStatus == AppSettings.StatusOrderCancelled)
        {
          data.CancelledDate = DateTime.Now;
        }
        // nếu data có orderStatus = DELIVERED thì mà người dùng update lại về các trạng thái khác thì sẽ không cho update
        if (data.OrderStatus == AppSettings.StatusOrderDelivered && model.OrderStatus != AppSettings.StatusOrderDelivered)
        {
          throw new BadRequestException(TRXTRANSACTIONDELIVERED);
        }
        data.OrderStatus = model.OrderStatus;
        _unitOfWork.TrxTransaction.Update(data);
        _unitOfWork.Save();
        _unitOfWork.Commit();
        ShoppingCartVM cartVM = new ShoppingCartVM();
        cartVM.TrxTransaction = data;
        strMessage = UPDATETRXTRANSACTIONSUCCESS;
        return cartVM;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDORDEROUT);
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
        throw;
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
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDUSEROUT);
        }
        customerTransactionDTO.Customer = customer;
        customerTransactionDTO.TrxTransactions = _unitOfWork.TrxTransaction.GetAll(u => u.CustomerId == customer.Id).OrderByDescending(u => u.OrderDate).ToList();
        return customerTransactionDTO;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

  }
}