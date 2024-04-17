namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Product : MasterEntity
  {
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public int TrademarkId { get; set; }

    [ForeignKey("TrademarkId")]
    [ValidateNever]
    public Trademark? Trademark { get; set; }

    [Required(ErrorMessage = "Danh mục là bắt buộc")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }

    [Required(ErrorMessage = "Màu sắc là bắt buộc")]
    [StringLength(255, ErrorMessage = "Màu sắc phải có tối đa 255 ký tự")]
    public string Color { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
    public decimal Price { get; set; } = 0;

    [Range(0, int.MaxValue, ErrorMessage = "Mức độ ưu tiên phải lớn hơn hoặc bằng 0")]
    public int? PriorityLevel { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
    public int Quantity { get; set; } = 0;

    [StringLength(255, ErrorMessage = "Ghi chú phải có tối đa 255 ký tự")]
    public string? Notes { get; set; } = string.Empty;

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;

    [NotMapped]
    public double? Discount { get; set; }

    [NotMapped]
    public double? BatchNumber { get; set; }

    [NotMapped]
    public string? Image { get; set; } = "https://localhost:7150";

    public void SetPromotion(double promotion)
    {
      Discount = promotion;
    }

  }
}
