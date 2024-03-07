using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class ProductDetailRepository : Repository<ProductDetail>, IProductDetailRepository
  {
    private PcsApiContext _db;
    public ProductDetailRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<ProductDetail, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(ProductDetail obj)
    {
      _db.ProductDetails.Update(obj);
    }
  }
}
