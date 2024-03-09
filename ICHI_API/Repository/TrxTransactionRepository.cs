using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class TrxTransactionRepository : Repository<TrxTransaction>, ITrxTransactionRepository
  {
    private PcsApiContext _db;
    public TrxTransactionRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<TrxTransaction, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(TrxTransaction obj)
    {
      _db.TrxTransactions.Update(obj);
    }
  }
}
