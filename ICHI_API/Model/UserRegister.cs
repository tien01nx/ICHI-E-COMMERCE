using System.ComponentModel.DataAnnotations;

namespace API.Model
{
  public class UserRegister
  {
    [StringLength(64)]
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường và 1 kí tự đặc biệt.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    public string? Role { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }

    public string? Address { get; set; } = string.Empty;
  }
}
