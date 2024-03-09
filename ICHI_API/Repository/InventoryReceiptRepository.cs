using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class InventoryReceiptRepository : Repository<InventoryReceipt>, IInventoryReceiptRepository
  {
    private PcsApiContext _db;
    public InventoryReceiptRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<InventoryReceipt, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(InventoryReceipt obj)
    {
      _db.InventoryReceipts.Update(obj);
    }
  }
}
