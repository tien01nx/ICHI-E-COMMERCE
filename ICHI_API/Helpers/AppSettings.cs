namespace ICHI_CORE.Helpers
{
    public class AppSettings
    {
        public static string ConnectionString = "";
        public static String ADMIN = "ADMIN";
        public static String USER = "USER";
        public static String EMPLOYEE = "EMPLOYEE";
        public static String FolderImage = @"/Uploads/";
        public static bool InitData(IConfiguration configuration)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(configuration.GetConnectionString("DefaultConnection")))
                    ConnectionString = configuration.GetConnectionString("DefaultConnection");
                return true;
            }
            catch (Exception ex)
            {
                //Logger.log.ErrorFormat("Get data from appSettings error: {0}", ex.ToString());
                return false;
            }
        }
    }
}
