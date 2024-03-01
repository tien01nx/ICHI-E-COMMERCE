using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain.MasterModel
{
  [Table("product_images")]
  public class ProductImages : MasterEntity
  {
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }
    public string ImageName { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }


    public string DisplayValue
    {
      get
      {
        return string.Format("{0} - {1}", Id, ImageName);
      }
    }
  }


}
