using Microsoft.EntityFrameworkCore;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_CORE.Entities
{
  public class PcsApiContext : DbContext
  {
    public PcsApiContext(DbContextOptions<PcsApiContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> CategoryProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Log>().HasNoKey();
      base.OnModelCreating(modelBuilder);
    }
  }
}
