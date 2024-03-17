using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;

namespace ICHI.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private PcsApiContext _db;

        public CartRepository(PcsApiContext db) : base(db)
        {
            _db = db;
        }

        public bool ExistsBy(Expression<Func<Cart, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public void Update(Cart obj)
        {
            _db.Carts.Update(obj);
        }
    }
}
