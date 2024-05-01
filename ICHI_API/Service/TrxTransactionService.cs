using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using OfficeOpenXml;
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
    public Helpers.PagedResult<TrxTransaction> GetAll(string name, string orderStatus, string paymentStatus, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.TrxTransactions.OrderByDescending(u => u.OrderDate).AsQueryable();
        // ép kiểu string sang int là từ name sang int

        if (!string.IsNullOrEmpty(name))
        {
          int trxTransactionId;
          if (int.TryParse(name, out trxTransactionId))
          {
            query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()) || e.Id == trxTransactionId);
          }
          else
          {
            query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
          }
        }
        if (!string.IsNullOrEmpty(orderStatus))
        {
          query = query.Where(e => e.OrderStatus.Contains(orderStatus));
        }

        if (!string.IsNullOrEmpty(paymentStatus))
        {
          query = query.Where(e => e.PaymentStatus.Contains(paymentStatus));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<TrxTransaction>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public TrxTransactionDTO Create(TrxTransactionDTO trxTransactionDTO, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        TrxTransaction trxTransaction = new TrxTransaction();
        int checkPromotion = trxTransactionDTO.Carts.Where(x => x.Discount > 0).Count();

        // kiểm tra thông tin product trong carts để kiểm tra còn trong chương trình khuyến mãi không
        var promotion = _promotionService.CheckPromotionActive().Select(x => x.ProductId);

        var cartProduct = trxTransactionDTO.Carts.Where(x => x.Discount > 0 && promotion.Contains(x.ProductId)).ToList();
        if (cartProduct.Count == 0 && checkPromotion > 0)
        {
          throw new BadRequestException(TRXTRANSACTIONPROMTION);
        }
        trxTransaction.CustomerId = GetCustomerId(trxTransactionDTO.CustomerId);
        trxTransaction.PriceShip = trxTransactionDTO.PriceShip ?? 0;
        trxTransaction.FullName = trxTransactionDTO?.FullName;
        trxTransaction.PhoneNumber = trxTransactionDTO?.PhoneNumber;
        trxTransaction.Address = trxTransactionDTO?.Address;
        trxTransaction.OrderDate = DateTime.Now;
        trxTransaction.OrderStatus = trxTransactionDTO.OrderStatus ?? "PENDING";

        if (trxTransactionDTO.OrderStatus == AppSettings.StatusOrderDelivered)
        {
          trxTransaction.OnholDate = DateTime.Now;
          trxTransaction.WaitingForPickupDate = DateTime.Now;
          trxTransaction.WaitingForDeliveryDate = DateTime.Now;
          trxTransaction.DeliveredDate = DateTime.Now;
        }
        trxTransaction.PaymentTypes = trxTransactionDTO.PaymentTypes;

        //nếu PaymentTypes = CASH thì trạng thái thanh toán là đã thanh toán
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

        //trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
        trxTransactionDTO.Amount = trxTransaction.OrderTotal;
        trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
        // lấy thông tin đơn hàng theo userid từ cart
        //foreach (var item in trxTransactionDTO.Carts)
        //{
        //  TransactionDetail trxTransactionDetail = new TransactionDetail();
        //  trxTransactionDetail.ProductId = item.ProductId;
        //  trxTransactionDetail.Quantity = item.Quantity;
        //  trxTransactionDetail.Price = item.Price;
        //  trxTransactionDetail.BatchNumber = GetBatchNumber(item.ProductId);
        //  trxTransactionDetail.TrxTransactionId = trxTransaction.Id;
        //  _unitOfWork.TransactionDetail.Add(trxTransactionDetail);
        //}
        foreach (var item in trxTransactionDTO.Carts)
        {
          // Lấy danh sách các lô và số lượng tương ứng
          var batchNumbers = GetBatchNumbers(item.ProductId, item.Quantity);

          foreach (var batch in batchNumbers)
          {
            TransactionDetail trxTransactionDetail = new TransactionDetail();
            trxTransactionDetail.ProductId = item.ProductId;
            trxTransactionDetail.Quantity = batch.Quantity;
            trxTransactionDetail.Price = item.Price;
            trxTransactionDetail.BatchNumber = batch.BatchNumber;
            trxTransactionDetail.TrxTransactionId = trxTransaction.Id;
            _unitOfWork.TransactionDetail.Add(trxTransactionDetail);
          }
        }

        if (checkPromotion > 0)
        {
          UpdatePromotionDetail(cartProduct);
        }

        var listCartId = trxTransactionDTO.Carts.Select(x => x.Id).ToList();
        _unitOfWork.Cart.RemoveRange(_unitOfWork.Cart.GetAll(u => listCartId.Contains(u.Id)));
        UpdateProductQuantity(trxTransactionDTO.Carts);
        _unitOfWork.Save();
        _unitOfWork.Commit();
        return trxTransactionDTO;
      }
      catch (Exception)
      {
        _unitOfWork.Rollback();
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

        switch (model.OrderStatus)
        {
          case "PENDING":
            data.OrderDate = DateTime.Now;
            break;
          case "DELIVERED":
            data.OnholDate = DateTime.Now;
            data.DeliveredDate = DateTime.Now;
            break;
          case "WAITINGFORPICKUP":
          case "WAITINGFORDELIVERY":
            data.WaitingForPickupDate = DateTime.Now;
            data.WaitingForDeliveryDate = DateTime.Now;
            break;
          case "CANCELLED":
            data.CancelledDate = DateTime.Now;
            break;
        }
        if (data.OrderStatus == AppSettings.StatusOrderDelivered && model.OrderStatus != AppSettings.StatusOrderDelivered)
        {
          throw new BadRequestException(TRXTRANSACTIONDELIVERED);
        }
        if (data.PaymentStatus == AppSettings.PaymentStatusPending && data.PaymentTypes == AppSettings.PaymentStatusPending)
        {
          throw new BadRequestException("Không thể cập nhật trạng thái đơn hàng");
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
      catch (Exception)
      {
        throw;
      }
    }

    public bool CannelOrder(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        _unitOfWork.BeginTransaction();
        var data = _unitOfWork.TrxTransaction.Get(u => u.Id == id);
        if (data == null)
        {
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDORDER);
        }
        if (data.OrderStatus == AppSettings.StatusOrderDelivered)
        {
          throw new BadRequestException(TRXTRANSACTIONDELIVERED);
        }
        if (data.PaymentStatus == AppSettings.PaymentStatusPending && data.PaymentTypes == AppSettings.PaymentStatusPending)
        {
          throw new BadRequestException("Không thể hủy đơn hàng");
        }
        data.OrderStatus = AppSettings.StatusOrderCancelled;
        data.CancelledDate = DateTime.Now;
        _unitOfWork.TrxTransaction.Update(data);
        _unitOfWork.Save();
        _unitOfWork.Commit();
        strMessage = "CANCELORDER";
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }

    // giảm số lượng product khi đã mua
    public void UpdateProductQuantity(List<Cart> carts)
    {
      try
      {
        foreach (var item in carts)
        {
          var product = _unitOfWork.Product.Get(u => u.Id == item.ProductId, tracked: true);
          product.Quantity -= item.Quantity;
          _unitOfWork.Product.Update(product);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }
    // update số lượng mã giảm giá khi đã dùng
    public void UpdatePromotionDetail(List<Cart> carts)
    {
      try
      {
        foreach (var item in carts)
        {
          var productId = _unitOfWork.PromotionDetail.Get(u => u.ProductId == item.ProductId, tracked: true);
          productId.UsedCodesCount += 1;
          _unitOfWork.PromotionDetail.Update(productId);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    // tách hàm truyền vào userId lấy ra customerid khách hàng
    public int GetCustomerId(string userId)
    {
      try
      {
        var role = _unitOfWork.UserRole.Get(u => u.UserId == userId, "Role,User");
        if (role == null)
        {
          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDUSER);
        }
        if (role.Role.RoleName == AppSettings.EMPLOYEE)
        {
          return _unitOfWork.Employee.Get(u => u.UserId == userId).Id;
        }
        return _unitOfWork.Customer.Get(u => u.UserId == userId).Id;
      }
      catch (Exception)
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
      catch (Exception)
      {
        throw;
      }
    }
    public List<(double BatchNumber, int Quantity)> GetBatchNumbers(int productId, int purchaseQuantity)
    {
      List<(double BatchNumber, int Quantity)> batchNumbers = new List<(double BatchNumber, int Quantity)>();
      try
      {
        // Lấy ra tổng số lượng sản phẩm đã bán
        var soldQuantity = _unitOfWork.TransactionDetail.GetAll(u => u.ProductId == productId).Sum(u => u.Quantity);

        // Lấy ra các số lô của sản phẩm
        var inventoryReceiptDetails = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.ProductId == productId).OrderBy(u => u.BatchNumber).ToList();

        int remainingQuantity = purchaseQuantity;
        int cumulativeQuantity = 0; // Điều này sẽ theo dõi số lượng tích lũy được bao phủ bởi mỗi đợt

        foreach (var item in inventoryReceiptDetails)
        {
          if (remainingQuantity <= 0)
            break;

          int previousCumulativeQuantity = cumulativeQuantity;
          cumulativeQuantity += item.Quantity;

          // Tính số lượng có sẵn trong đợt hiện tại
          int availableQuantity = Math.Max(0, item.Quantity - Math.Max(soldQuantity - previousCumulativeQuantity, 0));

          if (availableQuantity > 0 && remainingQuantity > 0)
          {
            int quantityToTake = Math.Min(availableQuantity, remainingQuantity);
            batchNumbers.Add((item.BatchNumber, quantityToTake));
            remainingQuantity -= quantityToTake;
          }
          // Cập nhật số lượng đã bán cho đợt tiếp theo
          soldQuantity = Math.Max(soldQuantity - item.Quantity, 0);
        }
      }
      catch (Exception)
      {
        throw;
      }

      return batchNumbers;
    }
    public OrderStatusVM GetOrderStatus(out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        OrderStatusVM orderStatusVM = new OrderStatusVM();
        var data = _unitOfWork.TrxTransaction.GetAll();
        orderStatusVM.Pending = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderPending).Count();
        orderStatusVM.OnHold = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderOnHold).Count();
        orderStatusVM.WaitingForPickup = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderWaitingForPickup).Count();
        orderStatusVM.WaitingForDelivery = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderWaitingForDelivery).Count();
        orderStatusVM.Delivered = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderDelivered).Count();
        orderStatusVM.Cancelled = _unitOfWork.TrxTransaction.GetAll(u => u.OrderStatus == AppSettings.StatusOrderCancelled).Count();
        return orderStatusVM;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public MoneyTotal getMonneyTotal(out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // thực hiện lấy tổng số tiền của đơn hàng theo trạng thái đã thanh toán và chưa thanh toán
        // Tổng số lượng đơn hàng
        // Doanh số đặt hàng
        // Thực thu
        // Thực chi
        // Lấy theo ngày hiện tại
        MoneyTotal moneyTotal = new MoneyTotal();
        var data = _unitOfWork.TrxTransaction.GetAll(u => u.OrderDate.Date == DateTime.Now.Date);
        moneyTotal.TotalOrder = data.Count();
        moneyTotal.TotalOrderAmount = data.Sum(u => u.OrderTotal);
        moneyTotal.TotalRealAmount = data.Where(u => u.PaymentStatus == AppSettings.PaymentStatusApproved).Sum(u => u.OrderTotal);
        moneyTotal.TotalRemainAmount = data.Where(u => u.PaymentStatus == AppSettings.PaymentStatusPending).Sum(u => u.OrderTotal);

        // Lấy theo ngày hôm trước
        var dataYesterday = _unitOfWork.TrxTransaction.GetAll(u => u.OrderDate.Date == DateTime.Now.AddDays(-1).Date);
        moneyTotal.TotalOrderYesterday = dataYesterday.Count();
        moneyTotal.TotalOrderAmountYesterday = dataYesterday.Sum(u => u.OrderTotal);
        moneyTotal.TotalRealAmountYesterday = dataYesterday.Where(u => u.PaymentStatus == AppSettings.PaymentStatusApproved).Sum(u => u.OrderTotal);
        moneyTotal.TotalRemainAmountYesterday = dataYesterday.Where(u => u.PaymentStatus == AppSettings.PaymentStatusApproved).Sum(u => u.OrderTotal);

        // Tính phần trăm so với hôm qua
        // moneyTotal.PercentTotalOrder = (moneyTotal.TotalOrder - moneyTotal.TotalOrderYesterday) / moneyTotal.TotalOrderYesterday * 100;
        // moneyTotal.PercentTotalOrderAmount = (moneyTotal.TotalOrderAmount - moneyTotal.TotalOrderAmountYesterday) / moneyTotal.TotalOrderAmountYesterday * 100;
        // moneyTotal.PercentTotalRealAmount = (moneyTotal.TotalRealAmount - moneyTotal.TotalRealAmountYesterday) / moneyTotal.TotalRealAmountYesterday * 100;
        // moneyTotal.PercentTotalRemainAmount = (moneyTotal.TotalRemainAmount - moneyTotal.TotalRemainAmountYesterday) / moneyTotal.TotalRemainAmountYesterday * 100;

        // Tính phần trăm tăng giảm số lượng đơn hàng
        //decimal percentOrderChange = ((decimal)(moneyTotal.TotalOrder - moneyTotal.TotalOrderYesterday) / (decimal)moneyTotal.TotalOrderYesterday) * 100;
        //char orderChangeIndicator = percentOrderChange < 0 ? '-' : '+';

        //// Tính phần trăm tăng giảm tổng giá trị đơn hàng
        //decimal percentAmountChange = ((moneyTotal.TotalOrderAmount - moneyTotal.TotalOrderAmountYesterday) / moneyTotal.TotalOrderAmountYesterday) * 100;
        //char amountChangeIndicator = percentAmountChange < 0 ? '-' : '+';

        //// Tính phần trăm tăng giảm tổng giá trị đơn hàng đã thanh toán
        //decimal percentRealAmountChange = ((moneyTotal.TotalRealAmount - moneyTotal.TotalRealAmountYesterday) / moneyTotal.TotalRealAmountYesterday) * 100;
        //char realAmountChangeIndicator = percentRealAmountChange < 0 ? '-' : '+';

        //// Tính phần trăm tăng giảm tổng giá trị đơn hàng chưa thanh toán
        //decimal percentRemainAmountChange = ((moneyTotal.TotalRemainAmount - moneyTotal.TotalRemainAmountYesterday) / moneyTotal.TotalRemainAmountYesterday) * 100;
        //char remainAmountChangeIndicator = percentRemainAmountChange < 0 ? '-' : '+';

        //// In ra phần trăm tăng giảm

        //moneyTotal.PercentTotalOrder = Math.Abs(moneyTotal.PercentTotalOrder);
        //moneyTotal.PercentTotalOrderAmount = Math.Abs(moneyTotal.PercentTotalOrderAmount);
        //moneyTotal.PercentTotalRealAmount = Math.Abs(moneyTotal.PercentTotalRealAmount);
        //moneyTotal.PercentTotalRemainAmount = Math.Abs(moneyTotal.PercentTotalRemainAmount);

        return moneyTotal;
      }
      catch (Exception)
      {
        throw;
      }
    }
    // tính doanh thu và chi phí theo từng tháng theo năm truyền vào
    // doanh thu lấy từ bảng trxtransaction
    // chi phí lấy từ bảng inventoryreceiptDetail
    public List<MoneyMonth> getMonneyTotalByMonth(int year, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        List<MoneyMonth> moneyMonths = new List<MoneyMonth>();
        for (int i = 1; i <= 12; i++)
        {
          MoneyMonth moneyMonth = new MoneyMonth();
          var data = _unitOfWork.TrxTransaction.GetAll(u => u.OrderDate.Month == i && u.OrderDate.Year == year && u.OrderStatus == AppSettings.StatusOrderDelivered && u.PaymentStatus == AppSettings.PaymentStatusApproved);
          moneyMonth.Month = i;
          moneyMonth.TotalOrderAmount = data.Sum(u => u.OrderTotal);
          // Lấy chi phí nhập hàng bằng cách lấy tổng số lượng sản phẩm nhập nhân với price
          var inventoryReceiptDetail = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.CreateDate.Month == i && u.CreateDate.Year == year);
          moneyMonth.TotalRealAmount = inventoryReceiptDetail.Sum(u => u.Quantity * u.Price);
          moneyMonths.Add(moneyMonth);
        }
        return moneyMonths;
      }
      catch (Exception)
      {
        throw;
      }
    }
    // API viết hàm tính lợi nhận theo từng tháng theo năm truyền vào
    // Chi phí = tổng số lượng sản phẩm nhập * giá nhập
    // Cách tính lợi nhuận ta dựa vào trong TransactionDetail  ta có được số lô, giá bán, số lượng
    // Lợi nhuận bằng = Giá bán * số lượng - ( từ sô lô => giá mua của sản phẩm đó trong  inventoryReceiptDetail) (số lượng * giá nhập)

    public List<MoneyMonth> getProfitByMonth(int year, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        year = 2024;
        List<MoneyMonth> moneyMonths = new List<MoneyMonth>();
        for (int i = 1; i <= 12; i++)
        {
          MoneyMonth moneyMonth = new MoneyMonth();
          decimal cost = _unitOfWork.InventoryReceiptDetail
              .GetAll(u => u.CreateDate.Year == year && u.CreateDate.Month == i)
              .Sum(u => u.Price * u.Quantity);
          decimal revenue = 0;

          // lấy danh sách trong chi tiết hóa đơn
          var orderDetail = _unitOfWork.TransactionDetail.GetAll(includeProperties: "TrxTransaction");
          var order = orderDetail.Where(u => u.TrxTransaction.OrderStatus == AppSettings.StatusOrderDelivered &&
                                              u.TrxTransaction.PaymentStatus == AppSettings.PaymentStatusApproved &&
                                              u.TrxTransaction.OrderDate.Year == year &&
                                              u.TrxTransaction.OrderDate.Month == i);
          foreach (var item in order)
          {
            var amountInventory = _unitOfWork.InventoryReceiptDetail.Get(u => u.ProductId == item.ProductId && u.BatchNumber == item.BatchNumber).Price;
            revenue += (item.Price * item.Quantity) - (amountInventory * item.Quantity);
          }
          moneyMonth.Month = i;
          moneyMonth.TotalOrderAmount = revenue;
          moneyMonth.TotalRealAmount = cost;
          moneyMonths.Add(moneyMonth);
        }
        return moneyMonths;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public byte[] GenerateExcelReport(int year)
    {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "TemplateExcel.xlsx");
      var fileInfo = new FileInfo(filePath);

      using (var package = new ExcelPackage(fileInfo))
      {
        // Get the worksheets
        var reportSheet = package.Workbook.Worksheets["Báo cáo"];
        var profitSheet = package.Workbook.Worksheets["Lợi nhuận"];


        //STT Ngày Nhập Số Lô Tên Sản phẩm    Giá Bán Giá Mua Số Lượng    Thành Tiền

        var data = _unitOfWork.TransactionDetail.GetAll(includeProperties: "TrxTransaction,Product");
        var order = data.Where(u => u.TrxTransaction.OrderStatus == AppSettings.StatusOrderDelivered &&
                                                           u.TrxTransaction.PaymentStatus == AppSettings.PaymentStatusApproved && u.TrxTransaction.OrderDate.Year == year).GroupBy(u => u.ProductId).Select(group => new
                                                           {
                                                             ProductId = group.Key,
                                                             TotalQuantity = group.Sum(item => item.Quantity),
                                                             OrderDate = group.FirstOrDefault().TrxTransaction.OrderDate,
                                                             BatchNumber = group.FirstOrDefault().BatchNumber,
                                                             ProductName = group.FirstOrDefault().Product.ProductName,
                                                             Price = group.FirstOrDefault().Price, // Giá bán
                                                             PriceInventory = _unitOfWork.InventoryReceiptDetail.Get(u => u.ProductId == group.Key && u.BatchNumber == group.FirstOrDefault().BatchNumber).Price, // Giá mua
                                                             Items = group.ToList() // Các chi tiết trong nhóm
                                                           });

        // Insert data for sales
        decimal salesTotal = 0;
        int salesRowStart = 5;
        foreach (var item in order)
        {

          reportSheet.Cells[salesRowStart, 1].Value = item.ProductId;
          reportSheet.Cells[salesRowStart, 2].Value = item.OrderDate;
          reportSheet.Cells[salesRowStart, 2].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM"; // Định dạng ngày tháng
          reportSheet.Cells[salesRowStart, 3].Value = item.BatchNumber;
          reportSheet.Cells[salesRowStart, 4].Value = item.ProductName;
          reportSheet.Cells[salesRowStart, 5].Value = item.Price;
          reportSheet.Cells[salesRowStart, 6].Value = item.PriceInventory;
          reportSheet.Cells[salesRowStart, 7].Value = item.TotalQuantity;
          reportSheet.Cells[salesRowStart, 8].Formula = $"E{salesRowStart}*G{salesRowStart}"; // Giá bán * Số lượng
          salesTotal += item.Price * item.TotalQuantity;
          salesRowStart++;
        }
        reportSheet.Cells[salesRowStart, 7].Value = "Thành tiền";
        reportSheet.Cells[salesRowStart, 8].Value = salesTotal;


        var data1 = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.CreateDate.Year == year, includeProperties: "Product").GroupBy(u => u.ProductId).Select(group => new
        {
          ProductId = group.Key,
          TotalPrice = group.Sum(item => item.Price * item.Quantity),
          TotalQuantity = group.Sum(item => item.Quantity),
          OrderDate = group.FirstOrDefault().CreateDate,
          BatchNumber = group.FirstOrDefault().BatchNumber,
          ProductName = group.FirstOrDefault().Product.ProductName,
          Price = group.FirstOrDefault().Price,
          Items = group.ToList() // Các chi tiết trong nhóm

        })
            .ToList();

        // Insert data for purchases
        decimal purchaseTotal = 0;
        int purchaseRowStart = 5; // Adjust based on actual content
        int columnOffset = 13;
        foreach (var item in data1)
        {
          int Index = 1;
          reportSheet.Cells[purchaseRowStart, columnOffset + 1].Value = Index;
          reportSheet.Cells[purchaseRowStart, columnOffset + 2].Value = item.OrderDate;
          reportSheet.Cells[purchaseRowStart, columnOffset + 2].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM"; // Định dạng ngày tháng
          reportSheet.Cells[purchaseRowStart, columnOffset + 3].Value = item.BatchNumber;
          reportSheet.Cells[purchaseRowStart, columnOffset + 4].Value = item.ProductName;
          reportSheet.Cells[purchaseRowStart, columnOffset + 5].Value = item.Price;
          reportSheet.Cells[purchaseRowStart, columnOffset + 6].Value = item.TotalQuantity;
          reportSheet.Cells[purchaseRowStart, columnOffset + 7].Formula = $"R{purchaseRowStart}*S{purchaseRowStart}"; // Giá mua * Số lượng
          purchaseTotal += item.Price * item.TotalQuantity;
          purchaseRowStart++;
          Index++;
        }

        reportSheet.Cells[purchaseRowStart, columnOffset + 6].Value = "Thành tiền";
        reportSheet.Cells[purchaseRowStart, columnOffset + 7].Value = purchaseTotal;



        profitSheet.Cells["C5"].Formula = "'Báo cáo'!H" + salesRowStart.ToString(); // Link total sales to profit sheet
        profitSheet.Cells["C6"].Formula = "'Báo cáo'!T" + purchaseRowStart.ToString(); // Link total purchases to profit sheet
        profitSheet.Cells["C7"].Formula = "C5-C6"; // Calculate profit


        var memoryStream = new MemoryStream();
        package.SaveAs(memoryStream);

        memoryStream.Position = 0;

        return memoryStream.ToArray();

      }
    }




  }
}