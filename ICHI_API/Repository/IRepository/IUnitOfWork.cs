using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IUnitOfWork
  {
    ICategoryRepository Category { get; }
    ICustomerRepository Customer { get; }
    IEmployeeRepository Employee { get; }
    IInventoryReceiptDetailRepository InventoryReceiptDetail { get; }
    IInventoryReceiptRepository InventoryReceipt { get; }
    IProductRepository Product { get; }
    IProductDetailRepository ProductDetail { get; }
    IProductReturnRepository ProductReturn { get; }
    IProductReturnDetailRepository ProductReturnDetail { get; }
    IProductImagesRepository ProductImages { get; }
    IPromotionRepository Promotion { get; }
    IPromotionDetailRepository PromotionDetail { get; }
    IUserRoleRepository UserRole { get; }
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    ISupplierRepository Supplier { get; }
    ITrademarkRepository Trademark { get; }
    ITransactionDetailRepository TransactionDetail { get; }
    ITrxTransactionRepository TrxTransaction { get; }

    void Dispose();
    void BeginTransaction();
    void Commit();
    void Rollback();
    void Save();
  }
}
