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


    // JWT
    public const string JWTKEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
    public const string JWTISSUER = "https://localhost:7287/";
    public const string JWTAUDIENCE = "https://localhost:7287/";
    public const int JWTEXPIREDAY = 10;

    // Gmail SMTP
    public const string GmailEmail = "accnichface03@gmail.com";
    public const string GmailPassword = "nllezcjrutgegavp";

    // Vnpay
    public const string VnpayTmnCode = "VDM3ZZT0";
    public const string VnpayHashSecret
      = "VPKWACIPMXJBNPKISVTHZSLJEWQPQVBK";
    public const string VnpayBaseUrl
      = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
    public const string VnpayCommand = "pay";
    public const string VnpayCurrCode = "VND";
    public const string VnpayVersion = "2.1.0";
    public const string VnpayLocale = "vn";
    public const string VnpayPaymentBackReturnUrl
      = "https://localhost:7150/api/TrxTransaction/CheckPayment";



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
