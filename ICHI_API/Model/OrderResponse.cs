using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class OrderResponse
  {

    public int Id { get; set; }
    public string Name { get; set; } // tên người nhận
    public string Address { get; set; }  // địa chỉ nhận
    public string Phone { get; set; } // số điện thoại nhận

    //public Date paymentDate; // ngày thanh toán

    public string PaymentTypes { get; set; } // Phương thức thanh toán
    public string PaymentStatus { get; set; } // Trạng thái thanh toán
    public DateTime DeliveredDate { get; set; } // ngày giao hàng
    public string Note { get; set; } // notes
    public string OrderStatus { get; set; } // Trạng thái đơn hàng

    public DateTime OrderDate { get; set; } // ngày đặt hàng
    public DateTime CancelledDate { get; set; } // ngày tra hàng
    public DateTime OnholDate { get; set; } // ngày xác nhận đơn hàng
    //public Date DeliveredDate { get; set; } // ngày giao hàng
    public string CancelReason { get; set; } // lý do hủy đơn hàng


    public decimal TotalMoney { get; set; } // tổng tiền đơn hàng
    public decimal TotalSaleMoney { get; set; } // tổng tiền sau khi sale
    public decimal TotalDiscount { get; set; } // tổng giảm giá
    public decimal TotalQuantity { get; set; } // tổng số lượng sản phẩm trong đơn hàng


    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }

    public int? EmployeeId { get; set; }
    public string employeeName { get; set; }


    // thông tin vận chuyển
    public string CarrierName { get; set; } // tên đơn vị vận chuyển
    public string CarrierLogo { get; set; } // logo đơn vị vận chuyển
    public string Service { get; set; } // dịch vụ vận chuyển
    public decimal? TotalFree { get; set; } // phí vận chuyển
    public List<OrderDetailsResponse> OrderDetailsResponses { get; set; }


    public static OrderResponse ConvertToModel(TrxTransaction trxTransaction, int productReturnCount)
    {
      OrderResponse orderResponse = new OrderResponse();
      orderResponse.Id = trxTransaction.Id;
      orderResponse.Name = trxTransaction.Customer?.FullName;
      orderResponse.Address = trxTransaction.Customer?.Address;
      orderResponse.Phone = trxTransaction.Customer?.PhoneNumber;
      orderResponse.PaymentTypes = trxTransaction.PaymentTypes;
      orderResponse.PaymentStatus = trxTransaction.PaymentStatus;
      orderResponse.DeliveredDate = trxTransaction.DeliveredDate;
      //orderResponse.Note = trxTransaction.Note;
      orderResponse.OrderStatus = trxTransaction.OrderStatus;
      orderResponse.OrderDate = trxTransaction.OrderDate;
      orderResponse.CancelledDate = trxTransaction.CancelledDate;
      orderResponse.OnholDate = trxTransaction.OnholDate;
      //orderResponse.cancelReason = trxTransaction.cancelReason;
      orderResponse.TotalMoney = trxTransaction.OrderTotal;

      orderResponse.TotalSaleMoney = trxTransaction.OrderTotal;
      orderResponse.TotalDiscount = 0;
      orderResponse.TotalQuantity = 0;


      orderResponse.CustomerId = trxTransaction.CustomerId;
      orderResponse.CustomerName = trxTransaction.Customer?.FullName;
      orderResponse.CustomerPhone = trxTransaction.Customer?.PhoneNumber;

      orderResponse.EmployeeId = trxTransaction?.EmployeeId;
      orderResponse.employeeName = trxTransaction?.Employee?.FullName;

      orderResponse.TotalFree = trxTransaction.PriceShip;
      orderResponse.OrderDetailsResponses = new List<OrderDetailsResponse>();
      foreach (var item in trxTransaction.TransactionDetails)
      {
        OrderDetailsResponse orderDetailsResponse = new OrderDetailsResponse();
        orderDetailsResponse.Id = item.Id;
        orderDetailsResponse.ProductId = item.ProductId;
        orderDetailsResponse.ProductImage = item.ProductImage;
        orderDetailsResponse.ProductName = item.Product.ProductName;
        orderDetailsResponse.ProductPrice = item.Price;
        orderDetailsResponse.Quantity = item.Quantity;
        orderDetailsResponse.SalePrice = item.SalePrice;


        if (item.SalePrice == 0)
        {
          orderDetailsResponse.TotalPrice = item.Price * item.Quantity;
        }
        else
        {
          orderDetailsResponse.TotalPrice = item.SalePrice * item.Quantity;
          orderResponse.TotalDiscount += (item.Price - item.SalePrice) * item.Quantity;
        }
        orderDetailsResponse.QuantityReturned = productReturnCount;

        orderResponse.OrderDetailsResponses.Add(orderDetailsResponse);
      }
      return orderResponse;
    }
  }

  public class OrderDetailsResponse
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; } // giá gốc
    public int Quantity { get; set; } // số lượng sản phẩm
    public decimal SalePrice { get; set; } // giá khuyến mãi
    public decimal TotalPrice { get; set; } // thành tiền
    public int QuantityReturned { get; set; } // số lượng trả lại
  }

}
