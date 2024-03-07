using ICHI_CORE.Domain.MasterModel;
namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IProductRepository : IRepository<Product>
  {
    void Update(Product obj);

  }
}
