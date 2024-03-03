using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class ProductReturnDetail : MasterEntity
  {
    public int TrxTransactionID { get; set; } = 0;
    [ForeignKey("TrxTransactionID")]
    [ValidateNever]
    public TrxTransaction? TrxTransaction { get; set; }
    public int ProductDetailID { get; set; } = 0;
    [ForeignKey("ProductDetailID")]
    [ValidateNever]
    public ProductDetail? ProductDetail { get; set; }



  }
}
