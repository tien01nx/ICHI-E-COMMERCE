#pragma warning disable SA1633 // File should have header
#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1633 // File should have header
#pragma warning disable SA1200 // Using directives should be placed correctly
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable SA1200, SA1600

namespace ICHI_CORE.Domain.MasterModel
{
    public class Cart : MasterEntity
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 0;
    }
}
#pragma warning restore SA1200 // Using directives should be placed correctly
