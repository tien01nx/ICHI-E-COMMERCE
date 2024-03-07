using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class CustomerRepository : Repository<Customer>, ICustomerRepository
  {
    private PcsApiContext _db;
    public CustomerRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Customer, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Customer obj)
    {
      _db.Customers.Update(obj);
    }
  }
}
