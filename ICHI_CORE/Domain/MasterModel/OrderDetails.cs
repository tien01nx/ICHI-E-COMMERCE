using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
    [Table("order_details")]
    public class OrderDetails :MasterEntity
    {
        public int OrderHeaderId { get; set; } 
        public int ProductId { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }

    }
}
