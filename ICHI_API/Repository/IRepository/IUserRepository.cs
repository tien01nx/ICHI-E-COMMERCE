using ICHI_CORE.Domain;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IUserRepository : IRepository<User>
  {
    DateTime GetTokenExpirationTime(string token);
    List<Claim> GetClaimsFromToken(string token);
    string GenerateWebToken(List<Claim> claims);
    string GenerateAccessToken(User user);
    void SetJWTCookie(string token);

    void Update(User obj);

    bool ExistsBy(Expression<Func<User, bool>> filter);

  }
}
