using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Cart : MasterEntity
  {
    public int UserId { get; set; } = 0;
    public int ProductId { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public int Quantity { get; set; } = 0;
  }
}
