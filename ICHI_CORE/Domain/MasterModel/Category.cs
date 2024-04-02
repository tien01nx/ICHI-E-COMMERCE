using System.ComponentModel.DataAnnotations;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Category : MasterEntity
  {
    [Required(ErrorMessage = "ID gốc là bắt buộc")]
    public int ParentID { get; set; }

    [Required(ErrorMessage = "Cấp độ danh mục là bắt buộc")]
    public int CategoryLevel { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên danh mục phải dài tối đa 255 ký tự")]
    public string CategoryName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
  }
}
