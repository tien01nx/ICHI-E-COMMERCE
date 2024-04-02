namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

  public class TransactionDetail : MasterEntity
  {
    [Required(ErrorMessage = "Mã hóa đơn là bắt buộc")]
    public int TrxTransactionId { get; set; } = 0;

    [ForeignKey("TrxTransactionId")]
    [ValidateNever]
    public TrxTransaction? TrxTransaction { get; set; }

    [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
    public int ProductId { get; set; } = 0;

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }

    [Required(ErrorMessage = "Giá là bắt buộc")]
    public decimal Price { get; set; } = 0;

    [Required(ErrorMessage = "Tổng là bắt buộc")]
    public int Total { get; set; } = 0;

    [NotMapped]
    public string? ProductImage { get; set; }

    public void SetProductImage(string imagePath)
    {
      ProductImage = imagePath;
    }
  }
}
