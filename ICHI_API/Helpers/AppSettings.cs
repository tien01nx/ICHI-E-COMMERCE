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

    // Trạng thái đơn hàng
    // Chưa thanh toán - Pending
    // Đã thanh toán - Approved

    public const string PaymentStatusPending = "Pending";
    public const string PaymentStatusApproved = "Approved";

    // Trạng thái đơn hàng
    // Chưa xác nhận - Pending
    // Chờ xác nhận - On hold
    // Chờ lấy hàng - WaitingForPickup
    // Chờ giao hàng - WaitingForDelivery
    // Đã giao hàng - Delivered
    // Đã hủy - Cancelled

    public static string StatusOrderPending = "Pending";
    public static string StatusOrderOnHold = "Onhold";
    public static string StatusOrderWaitingForPickup = "WaitingForPickup";
    public static string StatusOrderWaitingForDelivery = "WaitingForDelivery";
    public static string StatusOrderDelivered = "Delivered";
    public static string StatusOrderCancelled = "Cancelled";

    //  static paymentTypes = [
    //  { paymentTypes: 'PaymentOnDelivery', name: 'Thanh toán khi nhận hàng' },
    //  { paymentTypes: 'PaymentViaCard', name: 'Thanh toán qua thẻ' },
    //  { paymentTypes: 'Cash', name: 'Tiền mặt' },
    //];

    public static string PaymentOnDelivery = "PaymentOnDelivery";
    public static string PaymentViaCard = "PaymentViaCard";
    public static string Cash = "Cash";


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
