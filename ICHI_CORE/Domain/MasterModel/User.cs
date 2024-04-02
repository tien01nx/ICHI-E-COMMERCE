
namespace ICHI_CORE.Domain
{
  using System.ComponentModel.DataAnnotations;

  public class User
  {
    [Key]
    [StringLength(100, ErrorMessage = "Địa chỉ email phải có tối đa 100 ký tự")]
    public string Email { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(100, ErrorMessage = "Mật khẩu phải có tối đa 100 ký tự")]
    public string Password { get; set; } = string.Empty;

    public bool IsLocked { get; set; } = false;

    public int FailedPassAttemptCount { get; set; } = 0;

    [Required(ErrorMessage = "Ngày tạo là bắt buộc")]
    public DateTime CreateDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Người tạo là bắt buộc")]
    [StringLength(100, ErrorMessage = "Người tạo phải có tối đa 100 ký tự")]
    public string CreateBy { get; set; } = "Admin";

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    [StringLength(100, ErrorMessage = "Người sửa phải có tối đa 100 ký tự")]
    public string? ModifiedBy { get; set; } = "Admin";
  }
}
