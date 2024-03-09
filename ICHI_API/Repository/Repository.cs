using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly PcsApiContext _db;
    internal DbSet<T> dbSet;
    public Repository(PcsApiContext db)
    {
      _db = db;
      this.dbSet = _db.Set<T>();
      _db.Products.Include(u => u.Category).Include(u => u.Id);
    }
    public void Add(T entity)
    {
      dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {


      IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();
      query = query.Where(filter);

      if (!string.IsNullOrEmpty(includeProperties))
      {
        foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(property);
        }
      }

      return query.FirstOrDefault();

    }

    // category , Convertype
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
    {
      IQueryable<T> query = dbSet;
      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (!string.IsNullOrEmpty(includeProperties))
      {
        foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(property);

        }
      }
      return query.ToList();
    }

    public void Remove(T entity)
    {
      dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
      dbSet.RemoveRange(entity);
    }
  }
}
