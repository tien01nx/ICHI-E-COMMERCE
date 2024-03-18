using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
    public class CartDTO
    {
        public IEnumerable<Cart> Cart { get; set; }
        public List<ProductImages> ProductImages { get; set; }
    }
}
