using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class RoleRepository : Repository<Role>, IRoleRepository
  {
    private PcsApiContext _db;
    public RoleRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Role, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Role obj)
    {
      _db.Roles.Update(obj);
    }
  }
}
