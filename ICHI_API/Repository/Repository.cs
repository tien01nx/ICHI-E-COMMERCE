using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

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

    /// <summary>
    /// ví dụ: FindByCondition(new Dictionary<string, string> { { "Id", "1" }, { "Name", "Product 1" } });
    /// </summary>
    /// <param name="_params"></param>
    /// <returns></returns>
    public IEnumerable<T> FindByCondition(Dictionary<string, string> _params)
    {
      var data = _db.Set<T>().AsEnumerable();
      foreach (var pr in _params)
      {
        var properties = typeof(T).GetProperties();
        var property = properties.FirstOrDefault(x => x.Name.ToUpper() == pr.Key.ToUpper());
        if (property != null)
        {
          data = data.Where(x => property.GetValue(x)?.ToString() == pr.Value);
        }
      }
      return data;
    }


    /// <summary>
    /// ví dụ: FindBySQLRaw("SELECT * FROM Products WHERE Id=1");
    /// </summary>
    /// <param name="sqlRaw"></param>
    /// <returns></returns>
    public IEnumerable<T> FindBySQLRaw(string sqlRaw)
    {
      var data = _db.Set<T>().FromSqlRaw(sqlRaw);
      return data;
    }

    /// <summary>
    /// ví dụ: GetDataTableFromSQL("SELECT * FROM Products WHERE Id=1");
    /// có thể sử dụng cho các câu lệnh SQL phức tạp hoặc store procedure như: exec sp_GetAllProducts
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
    public DataTable GetDataTableFromSQL(string sqlQuery)
    {
      using (var command = _db.Database.GetDbConnection().CreateCommand())
      {
        command.CommandText = sqlQuery;
        command.CommandType = CommandType.Text;

        _db.Database.OpenConnection();

        using (var result = command.ExecuteReader())
        {
          var dataTable = new DataTable();
          dataTable.Load(result);
          return dataTable;
        }
      }
    }

    /// <summary>
    /// Get data by filter and include properties
    /// Vd: Get(u=>u.Id==1,"Category,ProductImage")
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <param name="tracked"></param>
    /// <returns></returns>

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
      IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

      if (!string.IsNullOrEmpty(includeProperties))
      {
        foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(property);
        }
      }
      query = query.Where(filter);
      return query.FirstOrDefault();
    }

    /// <summary>
    /// GetAll data by filter and include properties List
    /// Vd: GetAll(u=>u.Id==1,"Category,ProductImage")
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
    {
      IQueryable<T> query = dbSet;
      if (!string.IsNullOrEmpty(includeProperties))
      {
        foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(property);
        }
      }

      if (filter != null)
      {
        query = query.Where(filter);
      }
      return query.ToList();
    }


    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity"></param>
    public void Remove(T entity)
    {
      dbSet.Remove(entity);
    }

    /// <summary>
    /// Delete  list entity
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveRange(IEnumerable<T> entity)
    {
      dbSet.RemoveRange(entity);
    }

    /// <summary>
    /// hàm này thực hiện lấy dữ liệu theo key
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<T> GetByKeys(T entity)
    {
      //Get keys name
      var iKey = _db.Set<T>().EntityType.GetKeys().FirstOrDefault();
      int keyNumber = iKey.Properties.Count;
      object[] arrKeys = new object[keyNumber];
      int k = 0;
      foreach (var key in iKey.Properties)
      {
        arrKeys[k] = key.Name;
        k++;
      }

      // Get keys value
      object[] arrKeyValue = new object[keyNumber];
      for (int i = 0; i <= arrKeys.Count() - 1; i++)
      {
        arrKeyValue[i] = entity.GetType().GetProperty(arrKeys[i].ToString()).GetValue(entity, null);
      }

      // Get data by keys
      var objectByKey = await _db.Set<T>().FindAsync(arrKeyValue);
      return objectByKey;
    }

    /// <summary>
    /// hàm này thực hiện update nhiều bản ghi
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<List<T>> UpdateBatch(List<T> entities)
    {
      int updateSuccess = 0;
      try
      {
        if (entities != null && entities.Any())
        {
          foreach (var entity in entities)
          {
            try
            {
              var existingModel = await GetByKeys(entity);
              if (existingModel != null)
              {
                MapperHelper.Map(entity, existingModel);
                _db.Set<T>().Update(existingModel);
                updateSuccess++;
              }
            }
            catch (Exception e)
            {

            }
          }
          await _db.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        // Xử lý ngoại lệ nếu cần thiết
      }

      return entities.Take(updateSuccess).ToList();
    }

    public async Task<List<T>> CreateBatch(List<T> entitys)
    {
      int insertSuccess = 0;
      int insertError = 0;
      ApiResponse<T> result;
      try
      {
        if (entitys.Count > 0)
        {
          foreach (var entity in entitys)
          {
            try
            {
              var checkExits = await GetByKeys(entity);
              if (checkExits != null)
              {
                insertError++;
              }
              else
              {
                await _db.Set<T>().AddAsync(entity);
                insertSuccess++;
              }
            }
            catch (Exception e)
            {
              insertError++;
            }
          }
          await _db.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        // Xử lý ngoại lệ nếu cần thiết
      }
      return entitys.Take(insertSuccess).ToList();
    }
  }
}