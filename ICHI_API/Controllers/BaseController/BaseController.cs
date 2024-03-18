using API.Helpers;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using System.Net;
using ICHI_API;
using Microsoft.EntityFrameworkCore;
using ICHI_API.Data;

namespace ICHI_CORE.Controllers.BaseController
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly PcsApiContext _context;

        public BaseController(PcsApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get All resource
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public virtual async Task<ApiResponse<IEnumerable<T>>> FindAll()
        //{
        //  ApiResponse<IEnumerable<T>> result;
        //  try
        //  {
        //    var data = _context.Set<T>().AsEnumerable();
        //    result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.OK, "", data);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | FindAll | Exception: {1}", typeof(T).Name, ex.ToString());
        //    result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}


        ///// <summary>
        ///// Get resource by condition 
        ///// </summary>
        ///// <param name="_params">Name of property in resource</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("FindByCondition")]
        //public virtual async Task<ApiResponse<IEnumerable<T>>> FindByCondition(Dictionary<string, string> _params)
        //{
        //  ApiResponse<IEnumerable<T>> result;
        //  try
        //  {
        //    var data = _context.Set<T>().AsEnumerable();
        //    foreach (var pr in _params)
        //    {
        //      var properties = typeof(T).GetProperties();
        //      var property = properties.FirstOrDefault(x => x.Name.ToUpper() == pr.Key.ToUpper());
        //      if (property != null)
        //      {
        //        data = data.Where(x => property.GetValue(x)?.ToString() == pr.Value);
        //      }
        //    }

        //    if (data != null)
        //    {
        //      result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.OK, "", data);
        //    }
        //    else
        //    {
        //      result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.NoContent, "No found data.", null);
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | FindByCondition | Exception: {1}", typeof(T).Name, ex.ToString());
        //    NLogger.log.Error("{0} | FindByCondition | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(_params));
        //    result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Get resource by SQL String (vd: select * from [reosurceName] order by Id)
        ///// </summary>
        ///// <param name="sqlRaw"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("FindBySQLRaw")]
        //public virtual async Task<ApiResponse<IEnumerable<T>>> FindBySQLRaw(string sqlRaw)
        //{
        //  ApiResponse<IEnumerable<T>> result;
        //  try
        //  {

        //    var data = _context.Set<T>().FromSqlRaw(sqlRaw);
        //    if (data != null)
        //    {
        //      result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.OK, "", data);
        //    }
        //    else
        //    {
        //      result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.NoContent, "No found data.", null);
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | FindBySQLRaw | Exception: {1}", typeof(T).Name, ex.ToString());
        //    NLogger.log.Error("{0} | FindBySQLRaw | InputData: {1}", typeof(T).Name, sqlRaw);
        //    result = new ApiResponse<IEnumerable<T>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        //[HttpPost]
        //[Route("SaveBatch")]
        //public virtual async Task<ApiResponse<T>> SaveBatch(List<T> entitys)
        //{
        //  NLogger.log.Info("{0} | SaveBatch | TotalRecord: {1}", typeof(T).Name, entitys.Count);
        //  int insertNumber = 0;
        //  int updateNumber = 0;
        //  int errorNumber = 0;
        //  ApiResponse<T> result;
        //  try
        //  {
        //    if (entitys.Count > 0)
        //    {
        //      foreach (var entity in entitys)
        //      {
        //        try
        //        {
        //          var existingModel = await GetByKeys(entity);
        //          if (existingModel != null)
        //          {
        //            MapperHelper.Map(entity, existingModel);
        //            _context.Set<T>().Update(existingModel);
        //            updateNumber++;
        //          }
        //          else
        //          {
        //            await _context.Set<T>().AddAsync(entity);
        //            insertNumber++;
        //          }
        //        }
        //        catch (Exception e)
        //        {
        //          NLogger.log.Info("{0} | SaveBatch_Entity | Exception: {1}", typeof(T).Name, e.ToString());
        //          NLogger.log.Info("{0} | SaveBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //          errorNumber++;
        //        }
        //      }
        //      await _context.SaveChangesAsync();
        //      NLogger.log.Info("{0} | SaveBatch | InsertNumber: {1}", typeof(T).Name, insertNumber);
        //      NLogger.log.Info("{0} | SaveBatch | UpdateNumber: {1}", typeof(T).Name, updateNumber);
        //      NLogger.log.Info("{0} | SaveBatch | ErrorNumber: {1}", typeof(T).Name, errorNumber);
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, "", null);
        //    }
        //    else
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, string.Format("{0} | SaveBatch | Message: Input data is empty.", typeof(T).Name), null);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | SaveBatch | Exception: {1}", typeof(T).Name, ex.ToString());
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Create new resource
        ///// </summary>
        ///// <param name="entity">data resource</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Create")]
        //public virtual async Task<ApiResponse<T>> Create(T entity)
        //{
        //  ApiResponse<T> result;
        //  try
        //  {
        //    var checkExits = await GetByKeys(entity);
        //    if (checkExits != null)
        //    {
        //      NLogger.log.Info("{0} | Create | Message: The new record to add already exists.", typeof(T).Name);
        //      NLogger.log.Info("{0} | Create | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, "The new record to add already exists.", null);
        //    }
        //    else
        //    {
        //      await _context.Set<T>().AddAsync(entity);
        //      await _context.SaveChangesAsync();

        //      var data = await GetByKeys(entity);
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, "", data);
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error(ex.ToString());
        //    NLogger.log.Info("{0} | Create | Exception: {1}", typeof(T).Name, ex.ToString());
        //    NLogger.log.Info("{0} | Create | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Create new list resource
        ///// </summary>
        ///// <param name="entity">data resource</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("CreateBatch")]
        //public virtual async Task<ApiResponse<T>> CreateBatch(List<T> entitys)
        //{
        //  NLogger.log.Info("{0} | CreateBatch | TotalRecord: {1}", typeof(T).Name, entitys.Count);
        //  int insertSuccess = 0;
        //  int insertError = 0;
        //  ApiResponse<T> result;
        //  try
        //  {
        //    if (entitys.Count > 0)
        //    {
        //      foreach (var entity in entitys)
        //      {
        //        try
        //        {
        //          var checkExits = await GetByKeys(entity);
        //          if (checkExits != null)
        //          {
        //            NLogger.log.Error("{0} | CreateBatch_Entity | Message: The new record to add already exists.", typeof(T).Name);
        //            NLogger.log.Error("{0} | CreateBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //            insertError++;
        //          }
        //          else
        //          {
        //            await _context.Set<T>().AddAsync(entity);
        //            insertSuccess++;
        //          }
        //        }
        //        catch (Exception e)
        //        {
        //          NLogger.log.Error("{0} | CreateBatch_Entity | Exception: {1}", typeof(T).Name, e.ToString());
        //          NLogger.log.Error("{0} | CreateBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //          insertError++;
        //        }
        //      }
        //      await _context.SaveChangesAsync();
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, string.Format("Successfully added new {0}/{1} records.", insertSuccess, entitys.Count), null);
        //    }
        //    else
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, string.Format("{0} | CreateBatch | Message: Input data is empty.", typeof(T).Name), null);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | CreateBatch | Exception: {1}", typeof(T).Name, ex.ToString());
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Update resource
        ///// </summary>
        ///// <param name="entity">Data of resource to update</param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("Update")]
        //public virtual async Task<ApiResponse<T>> Update(T entity)
        //{
        //  ApiResponse<T> result;
        //  try
        //  {
        //    var existingModel = await GetByKeys(entity);
        //    if (existingModel == null)
        //    {
        //      NLogger.log.Info("{0} | Update | Message: Could not find a record to update.", typeof(T).Name);
        //      NLogger.log.Info("{0} | Update | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, "Could not find a record to update.", null);
        //    }
        //    else
        //    {
        //      MapperHelper.Map(entity, existingModel);
        //      _context.Set<T>().Update(existingModel);
        //      await _context.SaveChangesAsync();

        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, "", existingModel);
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Info("{0} | Update | Exception: {1}", typeof(T).Name, ex.ToString());
        //    NLogger.log.Info("{0} | Update | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Update list resource
        ///// </summary>
        ///// <param name="entity">Data of resource to update</param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("UpdateBatch")]
        //public virtual async Task<ApiResponse<T>> UpdateBatch(List<T> entitys)
        //{
        //  NLogger.log.Info("{0} | UpdateBatch | TotalRecord: {1}", typeof(T).Name, entitys.Count);
        //  int updateSuccess = 0;
        //  int updateError = 0;
        //  ApiResponse<T> result;
        //  try
        //  {
        //    if (entitys.Count > 0)
        //    {
        //      foreach (var entity in entitys)
        //      {
        //        try
        //        {
        //          var existingModel = await GetByKeys(entity);
        //          if (existingModel == null)
        //          {
        //            NLogger.log.Error("{0} | UpdateBatch_Entity | Message: Could not find a record to update.", typeof(T).Name);
        //            NLogger.log.Error("{0} | UpdateBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //            updateError++;
        //          }
        //          else
        //          {
        //            MapperHelper.Map(entity, existingModel);
        //            _context.Set<T>().Update(existingModel);
        //            updateSuccess++;
        //          }
        //        }
        //        catch (Exception e)
        //        {
        //          NLogger.log.Error("{0} | UpdateBatch_Entity | Exception: {1}", typeof(T).Name, e.ToString());
        //          NLogger.log.Error("{0} | UpdateBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //          updateError++;
        //        }
        //      }
        //      await _context.SaveChangesAsync();
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, string.Format("Successfully updated {0}/{1} records.", updateSuccess, entitys.Count), null);
        //    }
        //    else
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, string.Format("{0} | UpdateBatch | Message: Input data is empty.", typeof(T).Name), null);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | UpdateBatch | Exception: {1}", typeof(T).Name, ex.ToString());
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Delete resource
        ///// </summary>
        ///// <param name="id">Id of resource to delete</param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("Delete")]
        //public virtual async Task<ApiResponse<T>> Delete(T entity)
        //{
        //  ApiResponse<T> result;
        //  try
        //  {
        //    var checkExits = await GetByKeys(entity);
        //    if (checkExits == null)
        //    {
        //      NLogger.log.Info("{0} | Delete | Message: Could not find a record to delete.", typeof(T).Name);
        //      NLogger.log.Info("{0} | Delete | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, "Could not find a record to delete.", null);
        //    }
        //    else
        //    {
        //      _context.Set<T>().Remove(entity);
        //      await _context.SaveChangesAsync();

        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, "", null);
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Info("{0} | Delete | Exception: {1}", typeof(T).Name, ex.ToString());
        //    NLogger.log.Info("{0} | Delete | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Delete list resource
        ///// </summary>
        ///// <param name="id">Id of resource to delete</param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("DeleteBatch")]
        //public virtual async Task<ApiResponse<T>> DeleteBatch(List<T> entitys)
        //{
        //  NLogger.log.Info("{0} | DeleteBatch | TotalRecord: {1}", typeof(T).Name, entitys.Count);
        //  int totalSuccess = 0;
        //  int totalError = 0;
        //  ApiResponse<T> result;
        //  try
        //  {
        //    if (entitys.Count > 0)
        //    {
        //      foreach (T entity in entitys)
        //      {
        //        try
        //        {
        //          var checkExits = await GetByKeys(entity);
        //          if (checkExits == null)
        //          {
        //            NLogger.log.Error("{0} | DeleteBatch_Entity | Message: Could not find a record to delete.", typeof(T).Name);
        //            NLogger.log.Error("{0} | DeleteBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //            totalError++;
        //          }
        //          else
        //          {
        //            _context.Set<T>().Remove(entity);
        //            totalSuccess++;
        //          }
        //        }
        //        catch (Exception e)
        //        {
        //          NLogger.log.Error("{0} | DeleteBatch_Entity | Exception: {1}", typeof(T).Name, e.ToString());
        //          NLogger.log.Error("{0} | DeleteBatch_Entity | InputData: {1}", typeof(T).Name, JsonConvert.SerializeObject(entity));
        //          totalError++;
        //        }
        //      }
        //      await _context.SaveChangesAsync();
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.OK, string.Format("Successfully deleted {0}/{1} records", totalSuccess, entitys.Count), null);
        //    }
        //    else
        //      result = new ApiResponse<T>(System.Net.HttpStatusCode.NotFound, string.Format("{0} | DeleteBatch | Message: Input data is empty.", typeof(T).Name), null);
        //  }
        //  catch (Exception ex)
        //  {
        //    NLogger.log.Error("{0} | DeleteBatch | Exception: {1}", typeof(T).Name, ex.ToString());
        //    result = new ApiResponse<T>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
        //  }
        //  return result;
        //}

        ///// <summary>
        ///// Get data by keys
        ///// </summary>
        ///// <returns></returns>
        //protected async Task<T> GetByKeys(T entity)
        //{
        //  // Get keys name
        //  var iKey = _context.Set<T>().EntityType.GetKeys().FirstOrDefault();
        //  int keyNumber = iKey.Properties.Count;
        //  object[] arrKeys = new object[keyNumber];
        //  int k = 0;
        //  foreach (var key in iKey.Properties)
        //  {
        //    arrKeys[k] = key.Name;
        //    k++;
        //  }

        //  // Get keys value
        //  object[] arrKeyValue = new object[keyNumber];
        //  for (int i = 0; i <= arrKeys.Count() - 1; i++)
        //  {
        //    arrKeyValue[i] = entity.GetType().GetProperty(arrKeys[i].ToString()).GetValue(entity, null);
        //  }

        //  // Get data by keys
        //  var objectByKey = await _context.Set<T>().FindAsync(arrKeyValue);
        //  return objectByKey;
        //}
    }
}
