using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
  public class UserRegister
  {
    [StringLength(64)]
    [Required]
    public string UsePassword { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;


  }
}
