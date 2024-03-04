using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class TransactionDetail : MasterEntity
  {
    public int TrxTransactionID { get; set; } = 0;
    [ForeignKey("TrxTransactionID")]
    public TrxTransaction? TrxTransaction { get; set; }
    [Required]
    public decimal Price { get; set; } = 0;
    [Required]
    public int Total { get; set; } = 0;



  }
}
