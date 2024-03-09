using Microsoft.EntityFrameworkCore;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Data
{
  public class PcsApiContext : DbContext
  {
    public PcsApiContext(DbContextOptions<PcsApiContext> options) : base(options) { }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<InventoryReceipt> InventoryReceipts { get; set; }

    public DbSet<InventoryReceiptDetail> InventoryReceiptDetails { get; set; }
    public DbSet<Log> Logs { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }

    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<ProductReturn> ProductReturns { get; set; }
    public DbSet<ProductReturnDetail> ProductReturnDetails { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionDetail> PromotionDetails { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Trademark> Trademarks { get; set; }
    public DbSet<TrxTransaction> TrxTransactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Log>().HasNoKey();
      base.OnModelCreating(modelBuilder);
    }
  }
}
