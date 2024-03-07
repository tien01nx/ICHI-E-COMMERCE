using ICHI_CORE.Domain;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IUserRepository : IRepository<User>
  {

    void Update(User obj);

    bool ExistsBy(Expression<Func<User, bool>> filter);
    User ExistsByUserNameOrEmail(string userName);

  }
}
