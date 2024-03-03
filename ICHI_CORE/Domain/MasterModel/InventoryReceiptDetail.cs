using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class InventoryReceiptDetail : MasterEntity
  {
    [Required]
    public int InventoryReceiptID { get; set; } = 0;
    [ForeignKey("InventoryReceiptID")]
    public InventoryReceipt? InventoryReceipt { get; set; }
    [Required]
    public int ProductDetailID { get; set; } = 0;
    [ForeignKey("ProductDetailID")]
    public ProductDetail? ProductDetail { get; set; }
    [Required]
    public decimal Price { get; set; } = 0;
    [Required]
    public decimal Total { get; set; } = 0;
  }
}
