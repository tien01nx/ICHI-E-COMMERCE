using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain.MasterModel
{
  public class ProductImages : MasterEntity
  {
    public int ProductDetailId { get; set; }
    [ForeignKey("ProductDetailId")]
    [ValidateNever]
    public ProductDetail ProductDetail { get; set; }
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
