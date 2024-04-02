namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class ProductReturn : MasterEntity
  {
    [Required(ErrorMessage = "Người dùng là bắt buộc")]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }

    public bool isActive { get; set; } = false;
  }
}
