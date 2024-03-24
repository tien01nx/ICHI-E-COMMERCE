using ICHI_CORE.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IRepository<T> where T : class
  {
    T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    IEnumerable<T> FindByCondition(Dictionary<string, string> _params);
    IEnumerable<T> FindBySQLRaw(string sqlRaw);
    DataTable GetDataTableFromSQL(string sqlQuery);
    void Add(T entity);
    Task<List<T>> CreateBatch(List<T> entitys);
    Task<List<T>> UpdateBatch(List<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> filter);
    Task<T> GetByKeys(T entity);
  }
}
