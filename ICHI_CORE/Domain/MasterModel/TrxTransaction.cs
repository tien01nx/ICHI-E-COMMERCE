namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class TrxTransaction : MasterEntity
    {
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        [ValidateNever]

        public User? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShoppingDate { get; set; }

        [Required]
        public decimal OrderTotal { get; set; } = 0;

        [Required]
        public string OrderStatus { get; set; } = string.Empty;

        [Required]
        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public string PaymentIntentID { get; set; } = string.Empty;

        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(12)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
