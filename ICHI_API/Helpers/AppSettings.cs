namespace ICHI_CORE.Helpers
{
    public class AppSettings
    {
        public static string ConnectionString = "";
        public static String ADMIN = "ADMIN";
        public static String ACCOUNT_ADMIN = "admin@gmail.com";
        public static String USER = "USER";
        public static String EMPLOYEE = "EMPLOYEE";
        public static String FolderImage = @"/Uploads/";

        public static int PageNumber = 1;
        public static int PageSize = 10;
        public static string SortBy = "Id";
        public static string SortDirection = "asc";
        public static string Encode = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
        public static string AvatarDefault = "/images/avatar/avatar_default.jpg";
        public static string PatchProduct = @"images\products\product-";
        public static string PatchUser = @"images\user\user-";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";


        public const string SessionCart = "SessionShoppingCart";

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
