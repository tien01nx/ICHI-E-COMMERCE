//using Microsoft.Extensions.Caching.Memory;
//using Newtonsoft.Json;

//namespace ICHI_API.Helpers
//{
//  public class CacheHelper







//  {
//    private static CacheHelper? instance;
//    private bool isReady;
//    private MemoryCache andonMemory;
//    private CacheItemPolicy cacheItemPolicy;

//    public static CacheHelper Instance
//    {
//      get
//      {
//        if (instance == null)
//        {
//          instance = new CacheHelper();
//        }

//        return instance;
//      }
//    }

//    public bool IsReady
//    {
//      get => isReady;
//      set => isReady = value;
//    }

//    public MemoryCache AndonMemory
//    {
//      get => andonMemory;
//      set => andonMemory = value;
//    }

//    public CacheItemPolicy CacheItemPolicy
//    {
//      get => cacheItemPolicy;
//      set => cacheItemPolicy = value;
//    }

//    public CacheHelper()
//    {
//      IsReady = false;

//      // Initialize the memory cache
//      AndonMemory = new MemoryCache("AndonMemory");

//      // Initialize the cacheItemPolicy
//      cacheItemPolicy = new CacheItemPolicy();

//      // cacheItemPolicy = new CacheItemPolicy
//      // {
//      //    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(120.0),
//      //    SlidingExpiration = TimeSpan.Zero,
//      //    Priority = 0,
//      // };
//    }

//    public T GetByKey<T>(string systemId, string protocolNo)
//    {
//      try
//      {
//        if (instance == null)
//        {
//          instance = new CacheHelper();
//        }

//        if (string.IsNullOrWhiteSpace(systemId) || string.IsNullOrWhiteSpace(protocolNo))
//          return default(T);

//        string skey = string.Format(MemmoryKey.FormatKey, systemId, protocolNo);
//        var memmoryValue = (T)AndonMemory.Get(skey);
//        return memmoryValue;
//      }
//      catch (Exception)
//      {
//        return default(T);
//        throw;
//      }
//    }

//    /// <summary>
//    /// Add data to in memory cache.
//    /// </summary>
//    /// <param name="key">key.</param>
//    /// <param name="value">value.</param>
//    public void AddMemoryCache(string key, object value)
//    {
//      try
//      {
//        CacheItem cache = new CacheItem(key, value);

//        var isExitsCacheItem = AndonMemory.GetCacheItem(key);
//        if (isExitsCacheItem != null)
//        {
//          AndonMemory.Remove(key);
//        }

//        AndonMemory.Add(cache, cacheItemPolicy);
//      }
//      catch (Exception)
//      {
//        throw;
//      }

//    }

//    /// <summary>
//    /// Update data to memmory cache from database.
//    /// </summary>
//    /// <param name="strRecvMsg">Message data from database.</param>
//    /// <param name="strResMsg">Notify the results of the processing process.</param>
//    /// <returns>True:Success | False:Faile.</returns>
//    public bool UpdateMemoryData(string strRecvMsg, out string strResMsg)
//    {
//      strResMsg = "Updated data to memory cache successfully.";
//      try
//      {
//        if (string.IsNullOrWhiteSpace(strRecvMsg))
//          return false;

//        TcpClientDataModel paramsData = JsonConvert.DeserializeObject<TcpClientDataModel>(strRecvMsg);
//        if (instance == null)
//        {
//          instance = new CacheHelper();
//        }

//        string sKey = string.Format(MemmoryKey.FormatKey, paramsData.SystemId, paramsData.ProtocolNo);

//        AddMemoryCache(sKey, paramsData);

//        // Mark data in the Memmory cache as ready for use.
//        IsReady = true;
//        return true;
//      }
//      catch (Exception ex)
//      {
//        strResMsg = string.Format("Updating data into memory cache failed: {0}.", ex.Message);
//        return false;
//      }
//    }
//  }

//#pragma warning restore SA1600, SA1503, CS8600, CS8602, CS8603
//}