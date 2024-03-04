using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Category : MasterEntity
  {
    [Required]
    public int ParentID { get; set; }
    [Required]
    public int CategoryLevel { get; set; }
    [Required]
    [StringLength(255)]
    public string CategoryName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
  }
}
