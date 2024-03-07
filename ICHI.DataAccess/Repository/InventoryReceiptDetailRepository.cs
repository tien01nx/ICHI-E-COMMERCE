using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
  public class InventoryReceiptDetailRepository : Repository<InventoryReceiptDetail>, IInventoryReceiptDetailRepository
  {
    private PcsApiContext _db;
    public InventoryReceiptDetailRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<InventoryReceiptDetail, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(InventoryReceiptDetail obj)
    {
      _db.InventoryReceiptDetails.Update(obj);
    }
  }
}
