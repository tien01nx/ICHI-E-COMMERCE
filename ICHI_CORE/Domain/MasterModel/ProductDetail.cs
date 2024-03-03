using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class ProductDetail
  {
    public int ProductID { get; set; } = 0;
    public string Color { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
  }
}
