using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class CartProductDTO
  {
    public string email { get; set; }
    public List<Cart> carts { get; set; }
  }
}
