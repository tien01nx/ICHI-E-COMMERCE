using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain.MasterModel
{
  [Table("category_product")]
  public class CategoryProduct : MasterEntity
  {
    public int ParentID { get; set; }
    public int CategoryLevel { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

  }
}
