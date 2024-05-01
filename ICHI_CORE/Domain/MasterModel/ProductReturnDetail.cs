namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class ProductReturnDetail : MasterEntity
  {
    [Required(ErrorMessage = "Mã đổi trả bắt buộc")]
    public int ProductReturnId { get; set; } = 0;

    [ForeignKey("ProductReturnId")]
    [ValidateNever]
    public ProductReturn? ProductReturn { get; set; }

    [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
    public int ProductId { get; set; } = 0;

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 0;

    [StringLength(50)]
    [Required(ErrorMessage = "Lý do đổi trả")]
    public string Reason { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Trạng thái")]
    public bool ReturnType { get; set; }

  }
}
