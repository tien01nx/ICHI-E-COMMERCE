namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class TrxTransaction : MasterEntity
  {
    [Required(ErrorMessage = "Người dùng là bắt buộc")]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }

    [Required(ErrorMessage = "Ngày đặt hàng là bắt buộc")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Ngày mua hàng là bắt buộc")]
    public DateTime ShoppingDate { get; set; }

    [Required(ErrorMessage = "Tổng đơn hàng là bắt buộc")]
    public decimal OrderTotal { get; set; } = 0;

    [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc")]
    public string OrderStatus { get; set; } = string.Empty;

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
  }
}
