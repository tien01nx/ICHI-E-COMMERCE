namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

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

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Số mã sử dụng bắt buộc")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng mã sử dụng phải lớn hơn hoặc bằng 0")]
    public int? UsedCodesCount { get; set; } = 0;
  }
}
