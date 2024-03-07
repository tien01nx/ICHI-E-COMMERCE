using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    private PcsApiContext _db;
    public UserRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<User, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    // kiểm tra user có tồn tại không theo username hoặc email
    public User ExistsByUserNameOrEmail(string userName)
    {
      return dbSet.FirstOrDefault(x => x.UserName.ToLower().Equals(userName) || x.Email.Equals(userName));
    }

    public void Update(User obj)
    {
      _db.Users.Update(obj);
    }
  }
}
