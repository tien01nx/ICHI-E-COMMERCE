using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Employee : MasterEntity
  {
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }

    [Required(ErrorMessage = "Họ và tên đầy đủ là bắt buộc")]
    [StringLength(255, ErrorMessage = "Họ và tên đầy đủ phải có tối đa 255 ký tự")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    [StringLength(3, ErrorMessage = "Giới tính phải có tối đa 3 ký tự")]
    public string Gender { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ngày sinh")]
    public DateTime Birthday { get; set; }

    [StringLength(255, ErrorMessage = "Email phải có tối đa 255 ký tự")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [StringLength(12, ErrorMessage = "Số điện thoại phải có tối đa 12 ký tự")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Địa chỉ phải có tối đa 255 ký tự")]
    public string? Address { get; set; }

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;

    [StringLength(255, ErrorMessage = "Ảnh đại diện phải có tối đa 255 ký tự")]
    public string Avatar { get; set; } = string.Empty;
  }
}
