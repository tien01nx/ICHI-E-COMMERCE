using ICHI_API.Data;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_TEST
{
  public static class ContextGenerator
  {
    public static PcsApiContext Generator()
    {
      var options = new DbContextOptionsBuilder<PcsApiContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;
      return new PcsApiContext(options);
    }
    public static PcsApiContext GeneratorDb()
    {
      IConfiguration configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();
      if (AppSettings.InitData(configuration))
      {
        NLogger.log.Info("Get data from appSettings success.");
        NLogger.log.Error("Test log error");
      }
      else
      {
        NLogger.log.Error("Get data from appSettings error.");
      }

      var options = new DbContextOptionsBuilder<PcsApiContext>()
          .UseSqlServer(AppSettings.ConnectionString)
          .Options;
      return new PcsApiContext(options);
    }
  }
}
