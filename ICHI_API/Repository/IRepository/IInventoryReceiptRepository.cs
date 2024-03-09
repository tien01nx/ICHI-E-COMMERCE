using ICHI_CORE.Domain.MasterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IInventoryReceiptRepository : IRepository<InventoryReceipt>
  {

    void Update(InventoryReceipt obj);

    bool ExistsBy(Expression<Func<InventoryReceipt, bool>> filter);

  }
}
