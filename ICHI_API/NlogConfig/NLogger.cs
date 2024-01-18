using NLog;
using NLog.Web;

namespace ICHI_CORE.NlogConfig
{
    public class NLogger
    {
        public static readonly NLog.ILogger log = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    }
}
