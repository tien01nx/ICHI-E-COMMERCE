using ICHI_CORE.Domain.MasterModel;
using System.Linq.Expressions;
namespace ICHI.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        bool ExistsBy(Expression<Func<Product, bool>> filter);


    }
}
