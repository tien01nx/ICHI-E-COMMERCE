

using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class ProductReturnDetailRepository : Repository<ProductReturnDetail>, IProductReturnDetailRepository
  {
    private PcsApiContext _db;
    public ProductReturnDetailRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<ProductReturnDetail, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(ProductReturnDetail obj)
    {
      _db.ProductReturnDetails.Update(obj);
    }
  }
}
