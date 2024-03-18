namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class Cart : MasterEntity
    {
        [StringLength(100)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }


        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 0;

        [NotMapped]
        public string? ProductImage { get; set; }

        [NotMapped]
        public Customer? Customer { get; set; }

        public void SetProductImage(string imagePath)
        {
            ProductImage = imagePath;
        }

        public void SetCustomer(Customer customer)
        {
            Customer = customer;
        }
    }
}
