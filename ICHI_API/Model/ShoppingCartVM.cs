using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class ShoppingCartVM
  {
    public IEnumerable<Cart> Cart { get; set; }

    public Customer Customer { get; set; }

    public TrxTransaction TrxTransaction { get; set; }

    public IEnumerable<TransactionDetail>? TransactionDetail { get; set; }

  }
}
