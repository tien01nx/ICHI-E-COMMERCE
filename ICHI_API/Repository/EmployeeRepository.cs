using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
  {
    private PcsApiContext _db;
    public EmployeeRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Employee, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Employee obj)
    {
      _db.Employees.Update(obj);
    }
  }
}
