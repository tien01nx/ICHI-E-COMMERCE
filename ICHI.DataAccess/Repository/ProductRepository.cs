using ICHI.DataAccess.Data;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository
{
  public class ProductRepository : Repository<Product>, IProductRepository
  {
    private PcsApiContext _db;
    public ProductRepository(PcsApiContext db) : base(db)
    {
      _db = db;
    }

    public bool ExistsBy(Expression<Func<Product, bool>> filter)
    {
      return dbSet.Any(filter);
    }

    public void Update(Product obj)
    {
      _db.Products.Update(obj);
    }
  }
}
