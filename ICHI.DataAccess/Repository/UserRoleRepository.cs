using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
  {
    private PcsApiContext _db;
    public UserRoleRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<UserRole, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(UserRole obj)
    {
      _db.UserRoles.Update(obj);
    }
  }
}
