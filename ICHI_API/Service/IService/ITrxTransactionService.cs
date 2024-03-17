using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface ITrxTransactionService
    {
        PagedResult<TrxTransaction> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);

        TrxTransaction Create(TrxTransaction supplier, out string strMessage);

        TrxTransaction Update(TrxTransaction supplier, out string strMessage);

        TrxTransaction FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);

        Cart InsertCart(Cart cart, out string strMessage);
    }
}
