using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICHI_API.Model
{
    public class MoneyMonth
    {
        // tháng hiện tại
        public int Month { get; set; } = 1;
        // Doanh thu
        public decimal TotalOrderAmount { get; set; } = 0;
        // chi phí nhâp hàng
        public decimal TotalRealAmount { get; set; } = 0;
    }
}