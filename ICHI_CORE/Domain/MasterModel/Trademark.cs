namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;

  public class Trademark : MasterEntity
  {
    [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
    [StringLength(255, ErrorMessage = "Số điện thoại phải có tối đa 255 ký tự")]
    public string TrademarkName { get; set; } = string.Empty;
  }
}
