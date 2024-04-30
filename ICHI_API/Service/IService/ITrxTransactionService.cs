using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
    public interface ITrxTransactionService
    {
        Helpers.PagedResult<TrxTransaction> GetAll(string name, string orderStatus, string paymentStatus, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
        TrxTransactionDTO Create(TrxTransactionDTO trxTransactionDTO, out string strMessage);
        ShoppingCartVM Update(UpdateTrxTransaction model, out string strMessage);
        ShoppingCartVM GetTrxTransactionFindById(int id, out string strMessage);
        CustomerTransactionDTO GetCustomerTransaction(string userid, out string strMessage);
        int GetCustomerId(string userId);
        OrderStatusVM GetOrderStatus(out string strMessage);
        MoneyTotal getMonneyTotal(out string strMessage);
        List<MoneyMonth> getMonneyTotalByMonth(int year, out string strMessage);

        List<MoneyMonth> getProfitByMonth(int year, out string strMessage);
        byte[] GenerateExcelReport(int year);
    }
}
