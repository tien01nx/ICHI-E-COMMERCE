using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class PromotionRepository : Repository<Promotion>, IPromotionRepository
  {
    private PcsApiContext _db;
    public PromotionRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Promotion, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Promotion obj)
    {
      _db.Promotions.Update(obj);
    }
  }
}
