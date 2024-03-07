using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class ProductImagesRepository : Repository<ProductImages>, IProductImagesRepository
  {
    private PcsApiContext _db;
    public ProductImagesRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<ProductImages, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(ProductImages obj)
    {
      _db.ProductImages.Update(obj);
    }
  }
}
