using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class ProductReturnRepository : Repository<ProductReturn>, IProductReturnRepository
  {
    private PcsApiContext _db;
    public ProductReturnRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<ProductReturn, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(ProductReturn obj)
    {
      _db.ProductReturns.Update(obj);
    }
  }
}
