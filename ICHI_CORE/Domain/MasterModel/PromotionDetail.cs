namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class PromotionDetail : MasterEntity
  {
    [Required(ErrorMessage = "Mã Chương trình khuyến mãi là bắt buộc")]
    public int PromotionId { get; set; } = 0;

    [ForeignKey("PromotionId")]
    [ValidateNever]
    public Promotion? Promotion { get; set; }

    [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
    public int ProductId { get; set; } = 0;

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }
  }
}
