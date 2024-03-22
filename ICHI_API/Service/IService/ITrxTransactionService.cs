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
        TrxTransactionDTO InsertTxTransaction(TrxTransactionDTO trxTransactionDTO, out string strMessage);
        ShoppingCartVM GetTrxTransactionFindById(int id, out string strMessage);

    }
}
