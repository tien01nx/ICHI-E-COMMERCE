namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  public class Customer : MasterEntity
  {
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }

    [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên đầy đủ phải dài tối đa 255 ký tự")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    [StringLength(3, ErrorMessage = "Giới tính phải dài tối đa 3 ký tự")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
    [DataType(DataType.Date, ErrorMessage = "Định dạng ngày sinh không hợp lệ")]
    public DateTime Birthday { get; set; }

    [StringLength(12, ErrorMessage = "Số điện thoại phải dài tối đa 12 ký tự")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Địa chỉ phải dài tối đa 255 ký tự")]
    public string Address { get; set; } = string.Empty;

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;
  }
}