using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
    public interface ICartService
    {
        Cart InsertCard(Cart cart, out string strMessage);
        IEnumerable<Cart> GetCarts(string email, out string strMessage);

        Cart DeleteCart(Cart cart, out string strMessage);

        ShoppingCartVM GetShoppingCart(string email, List<Cart> carts, out string strMessage);

        Cart UpdateCart(Cart cart, out string strMessage);

        IEnumerable<Cart> CheckCartPromotion(List<Cart> carts, out string strMessage);
        IEnumerable<TransactionDetail> CheckCartPromotion(int trxTransactionId, out string strMessage);

    }
}
