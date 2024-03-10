using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
  public class UserRegister
  {
    [StringLength(64)]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }


  }
}
