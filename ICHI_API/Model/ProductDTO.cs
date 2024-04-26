using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class ProductDTO
  {
    public Product Product { get; set; }
    public IEnumerable<ProductImages> ProductImages { get; set; }
    public Category CategoryProduct { get; set; }
    public PromotionDetail PromotionDetail { get; set; }
  }
}
