using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class PromotionDetailRepository : Repository<PromotionDetail>, IPromotionDetailRepository
  {
    private PcsApiContext _db;
    public PromotionDetailRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<PromotionDetail, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(PromotionDetail obj)
    {
      _db.PromotionDetails.Update(obj);
    }
  }
}
