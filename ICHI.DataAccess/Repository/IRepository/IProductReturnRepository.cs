using ICHI_CORE.Domain.MasterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IProductReturnRepository : IRepository<ProductReturn>
  {

    void Update(ProductReturn obj);

    bool ExistsBy(Expression<Func<ProductReturn, bool>> filter);

  }
}
