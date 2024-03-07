using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class CategoryRepository : Repository<Category>, ICategoryRepository
  {
    private PcsApiContext _db;
    public CategoryRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Category, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Category obj)
    {
      _db.Categories.Update(obj);
    }
  }
}
