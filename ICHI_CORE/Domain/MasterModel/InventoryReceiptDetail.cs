namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class InventoryReceiptDetail : MasterEntity
  {
    public int InventoryReceiptId { get; set; } = 0;

    [ForeignKey("InventoryReceiptId")]
    public InventoryReceipt? InventoryReceipt { get; set; }

    [Required]
    public int ProductId { get; set; } = 0;

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    public decimal Price { get; set; } = 0;

    public double Total { get; set; } = 0;
  }
}
