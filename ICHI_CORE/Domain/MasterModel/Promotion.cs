using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Promotion : MasterEntity
  {
    [Required]
    [StringLength(50)]
    public string PromotionCode { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string PromotionName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Quantity { get; set; }
    public bool isActive { get; set; } = false;
    public bool isDeleted { get; set; } = false;

  }
}
