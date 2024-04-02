namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;

  public class Promotion : MasterEntity
  {
    [Required(ErrorMessage = "Tên chương trình khuyến mãi là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên chương trình khuyến mãi phải có tối đa 255 ký tự")]
    public string PromotionName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
    //[DataType(DataType.DateTime, ErrorMessage = "Thời gian không hợp lệ")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
    //[DataType(DataType.DateTime, ErrorMessage = "Thời gian không hợp lệ")]
    public DateTime EndTime { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Giảm giá là bắt buộc")]
    [Range(0, double.MaxValue, ErrorMessage = "Giảm giá phải lớn hơn hoặc bằng 0")]
    public double Discount { get; set; }

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;
  }
}
