using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class TrademarkRepository : Repository<Trademark>, ITrademarkRepository
  {
    private PcsApiContext _db;
    public TrademarkRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Trademark, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Trademark obj)
    {
      _db.Trademarks.Update(obj);
    }
  }
}
