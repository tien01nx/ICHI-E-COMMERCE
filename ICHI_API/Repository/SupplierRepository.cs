using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class SupplierRepository : Repository<Supplier>, ISupplierRepository
  {
    private PcsApiContext _db;
    public SupplierRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Supplier, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Supplier obj)
    {
      _db.Suppliers.Update(obj);
    }
  }
}
