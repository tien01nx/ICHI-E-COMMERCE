using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
    public class OrderHeaders :MasterEntity
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime ShoopingDate { get; set; }
        public float OrderTotal { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public DateTime PaymentData { get; set; }
        public DateTime PaymentDuaDate { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;


    }
}
