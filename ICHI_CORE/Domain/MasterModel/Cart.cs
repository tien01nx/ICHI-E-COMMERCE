namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class Cart : MasterEntity
    {
        [Required(ErrorMessage = "Id người dùng là bắt buộc")]
        [StringLength(100, ErrorMessage = "UserId phải dài tối đa 100 ký tự")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }

        [Required(ErrorMessage = "ID sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; } = 0;

        [NotMapped]
        public string? ProductImage { get; set; }

        [NotMapped]
        public Customer? Customer { get; set; }

        [NotMapped]
        public double? Discount { get; set; } = 0;

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
