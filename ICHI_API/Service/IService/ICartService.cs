using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface ICartService
    {
        Cart InsertCard(Cart cart, out string strMessage);
        IEnumerable<Cart> GetCarts(string email, out string strMessage);

        Cart DeleteCart(Cart cart, out string strMessage);

        ShoppingCartVM GetShoppingCart(string email, out string strMessage);


    }
}
