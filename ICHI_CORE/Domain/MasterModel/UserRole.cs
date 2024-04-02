namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class UserRole
  {
    public int Id { get; set; } = 0;

    public int RoleId { get; set; } = 0;

    [ForeignKey("RoleId")]
    [ValidateNever]
    public Role? Role { get; set; }

    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }
  }
}
