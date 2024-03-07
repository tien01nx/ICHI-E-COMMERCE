using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class TransactionDetailRepository : Repository<TransactionDetail>, ITransactionDetailRepository
  {
    private PcsApiContext _db;
    public TransactionDetailRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<TransactionDetail, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(TransactionDetail obj)
    {
      _db.TransactionDetails.Update(obj);
    }
  }
}
