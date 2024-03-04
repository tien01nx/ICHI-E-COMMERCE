using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Cart : MasterEntity
  {
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }
    public int ProductDetailId { get; set; }
    [ForeignKey("ProductDetailId")]
    [ValidateNever]
    public ProductDetail? ProductDetail { get; set; }
    public decimal Price { get; set; } = 0;
    public int Quantity { get; set; } = 0;
  }
}
