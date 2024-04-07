using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
    public class CustomerTransactionDTO
    {
        public Customer Customer { get; set; }
        public List<TrxTransaction> TrxTransactions { get; set; }
    }
}
