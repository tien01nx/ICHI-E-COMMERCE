using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Trademark : MasterEntity
  {
    [Required]
    [StringLength(255)]
    public string TrademarkName { get; set; } = string.Empty;
  }
}
