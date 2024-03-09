using ICHI_CORE.Domain.MasterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface ITrxTransactionRepository : IRepository<TrxTransaction>
  {

    void Update(TrxTransaction obj);

    bool ExistsBy(Expression<Func<TrxTransaction, bool>> filter);

  }
}
