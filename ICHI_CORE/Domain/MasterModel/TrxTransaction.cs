namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Text.Json.Serialization;

  public class TrxTransaction : MasterEntity
  {

    [Required(ErrorMessage = "Khách hàng là bắt buộc")]

    public int CustomerId { get; set; } = 0;

    [ForeignKey("CustomerId")]
    [ValidateNever]
    public Customer? Customer { get; set; }

    public int? EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    [ValidateNever]
    public Employee? Employee { get; set; }

    // ngày đặt hàng
    [Required(ErrorMessage = "Ngày đặt hàng là bắt buộc")]
    public DateTime OrderDate { get; set; }

    // ngày xác nhận
    public DateTime OnholDate { get; set; }

    // ngày lấy hàng
    public DateTime WaitingForPickupDate { get; set; }

    // ngày giao hàng
    public DateTime WaitingForDeliveryDate { get; set; }

    // Đã giao hàng
    public DateTime DeliveredDate { get; set; }

    // Đã hủy
    public DateTime CancelledDate { get; set; }

    public decimal? PriceShip { get; set; }

    [Required(ErrorMessage = "Tổng đơn hàng là bắt buộc")]
    public decimal OrderTotal { get; set; } = 0;

    [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc")]
    public string OrderStatus { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc")]

    public string PaymentTypes { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trạng thái thanh toán là bắt buộc")]
    public string PaymentStatus { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; }

    public string SessionId { get; set; } = string.Empty;

    public string PaymentIntentID { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Họ và tên phải có tối đa 255 ký tự")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(12, ErrorMessage = "Số điện thoại phải có tối đa 12 ký tự")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Địa chỉ phải có tối đa 255 ký tự")]
    public string Address { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    [ValidateNever]
    [JsonIgnore]
    public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
  }
}
