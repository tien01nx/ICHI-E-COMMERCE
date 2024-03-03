using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class PromotionDetail : MasterEntity
  {
    public int PromotionID { get; set; } = 0;
    [ForeignKey("PromotionID")]
    [ValidateNever]
    public Promotion? Promotion { get; set; }

    public int ProductID { get; set; } = 0;
    [ForeignKey("ProductID")]
    [ValidateNever]
    public Product? Product { get; set; }
    public decimal Discount { get; set; } = 0;

  }
}
