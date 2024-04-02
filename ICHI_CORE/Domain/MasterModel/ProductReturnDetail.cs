namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class ProductReturnDetail : MasterEntity
  {
    [Required(ErrorMessage = "Hóa đơn là bắt buộc")]
    public int TrxTransactionId { get; set; } = 0;

    [ForeignKey("TrxTransactionId")]
    [ValidateNever]
    public TrxTransaction? TrxTransaction { get; set; }

    [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
    public int ProductId { get; set; } = 0;

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }

  }
}
