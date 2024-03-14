using Newtonsoft.Json;

namespace ICHI_API.Helpers
{
  public static class JsonHelper
  {
    //public static string rootFolder = ConfigurationManager.ConnectionStrings["pcs_app_data_offline"].ConnectionString;
    public static string rootFolder = "App_Data_Offline";

    public static T? ConvertJsonToModel<T>(string jsonPath)
    {
      try
      {
        if (File.Exists(jsonPath))
        {
          string strJson = string.Empty;
          strJson = File.ReadAllText(jsonPath);
          if (!string.IsNullOrEmpty(strJson))
          {
            return JsonConvert.DeserializeObject<T>(strJson);
          }
          else
          {
          }

        }
        else
        {
        }
      }
      catch (Exception ex)
      {
      }

      return default(T);
    }

    /// <summary>
    /// Save data from database to file Json
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    /// <param name="dataJson">dataJson</param>
    public static void SaveDataToFile<T>(string dataJson)
    {
      try
      {
        if (dataJson == null)
          dataJson = string.Empty;

        string filename = typeof(T).Name + ".txt";
        string filePath = Path.Combine(rootFolder, filename);
        if (!Directory.Exists(rootFolder))
          Directory.CreateDirectory(rootFolder);
        if (File.Exists(filePath))
        {
          File.WriteAllText(filePath, string.Empty);
        }

        File.WriteAllText(filePath, dataJson);
      }
      catch (Exception ex)
      {
      }
    }

    /// <summary>
    /// Get data from file json
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public static List<T>? GetDataFromFile<T>()
    {
      try
      {
        var a = typeof(T);
        string filename = typeof(T).Name + ".txt";
        string filePath = Path.Combine(rootFolder, filename);

        if (!File.Exists(filePath))
        {
          return default(List<T>);
        }

        string dataJson = File.ReadAllText(filePath);
        if (!string.IsNullOrEmpty(dataJson))
        {
          return JsonConvert.DeserializeObject<List<T>>(dataJson);
        }
        else
        {
          return default(List<T>);
        }
      }
      catch (Exception ex)
      {
        return default(List<T>);
      }
    }

    public static async Task<List<T>?> GetDataFromFileAsync<T>()
    {
      try
      {
        var fileName = typeof(T).Name + ".txt";
        var filePath = Path.Combine(rootFolder, fileName);

        if (!File.Exists(filePath))
        {
          return default;
        }

        string dataJson;
        using (var reader = new StreamReader(filePath))
        {
          dataJson = await reader.ReadToEndAsync();
        }

        if (!string.IsNullOrEmpty(dataJson))
        {
          return JsonConvert.DeserializeObject<List<T>>(dataJson);
        }

        return default;
      }
      catch (Exception ex)
      {
        return default;
      }
    }
  }
}
