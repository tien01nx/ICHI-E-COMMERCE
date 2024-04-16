using ICHI_CORE.Model;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

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

    public const string PaymentStatusPending = "PENDING";
    public const string PaymentStatusApproved = "APPROVED";

    // Trạng thái đơn hàng
    // Chưa xác nhận - Pending
    // Chờ xác nhận - On hold
    // Chờ lấy hàng - WaitingForPickup
    // Chờ giao hàng - WaitingForDelivery
    // Đã giao hàng - Delivered
    // Đã hủy - Cancelled

    public static string StatusOrderPending = "PENDING";
    public static string StatusOrderOnHold = "ONHOLD";
    public static string StatusOrderWaitingForPickup = "WAITINGFORPICKUP";
    public static string StatusOrderWaitingForDelivery = "WAITINGFORDELIVERY";
    public static string StatusOrderDelivered = "DELIVERED";
    public static string StatusOrderCancelled = "CANCELLED";

    //  static paymentTypes = [
    //  { paymentTypes: 'PaymentOnDelivery', name: 'Thanh toán khi nhận hàng' },
    //  { paymentTypes: 'PaymentViaCard', name: 'Thanh toán qua thẻ' },
    //  { paymentTypes: 'Cash', name: 'Tiền mặt' },
    //];

    public static string PaymentOnDelivery = "PAYMENTONDELIVERY";
    public static string PaymentViaCard = "PAYMENTVIACARD";
    public static string Cash = "CASH";


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

    public static async Task<ApiResponseGoShip<T>> CallPcsApi_NotRetry<T>(string actionAPI, Method method, Dictionary<string, object> _params = null, string bodyInput = null)
    {
      ApiResponseGoShip<T> result = null;
      string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjdkOGU1NDhjOGNhZmJjYWQxNTAxNmNkYmNkMmJiNWIwNDJiZWVlNDYyMjMxMjU4MzM2ZDEyYmE5YTg2MWQ5ZjJhZWFiNTZiMjllNmE0YzBmIn0.eyJhdWQiOiI3OSIsImp0aSI6IjdkOGU1NDhjOGNhZmJjYWQxNTAxNmNkYmNkMmJiNWIwNDJiZWVlNDYyMjMxMjU4MzM2ZDEyYmE5YTg2MWQ5ZjJhZWFiNTZiMjllNmE0YzBmIiwiaWF0IjoxNzEzMTI1NjU4LCJuYmYiOjE3MTMxMjU2NTgsImV4cCI6NDg2ODc5OTI1OCwic3ViIjoiMzI3MiIsInNjb3BlcyI6W119.N1WpQfpBmZfoaJkjDZ8vXEDZ6x9puAn31ttni061X_YQtLTNqNxHxNO_hFgS_WXyP8RVShDG-qdY16yLhYEkByidMPrt8DJZinPJRf_4jTa7KyZfpUWLtZiXdRfaD1Ci-fiyccks4YsyzcMKCneWLFbJtbmVctq8m4VbJz-x5t-vw4nRy40kKiWLYXt7952q_HJcqkQ-zS_ayjLV3obiLk9wtiyaQM07FfM5oSgvJVgekIN16BSA5p6MuHCGSzHnDSXLlfcgMfFw3NghRGfqjySpaORb5DsTk7tXe5-ssUy_GbP-F7lcAbnJ0735yyPWFXj75DDCEWfHvQX_8KHjKPO1d1nhGsoIkomQF5PyqRvyvc5L7szQbKo8hGXESm3ngXfPjc9QciIn1A3gWOI2I8eQoe8bwDiWGdTiMJfWH8ZCSQHlqxLKtnebhbxMntNnCeQmcqm4pmB04VDqDO0V9v7WrQJboahuZ8Qicbup8QGWYd7rmqpsBqLgdw37VSRuVCH9nIiaW4lcmAWBLXSeJrL6FHwJSzruCros80l-w4nlnpGkT5x8XBqnkoxCxOg31eMfTXj9C9DPCMs3_RtRv--qmCo8WlZFQcLz8WL5-6z7EVMzWn-mZ7WfsCoBK63Ly5xLsL8HXgLqPzTxD316lJatCSlCH6LgEHWXzrtL9UM";
      string strURL = "http://sandbox.goship.io/api";
      try
      {
        RestClient client = new RestClient(strURL);
        var request = new RestRequest(actionAPI, method);
        request.Timeout = 2000;
        request.AddHeader("Authorization", "Bearer " + token);
        request.AddHeader("Content-Type", "application/json");

        // add Parameter
        if (_params != null && _params.Count > 0)
        {
          foreach (var pr in _params)
          {
            request.AddParameter(pr.Key, pr.Value.ToString());
          }
        }
        // Add Body
        if (!string.IsNullOrEmpty(bodyInput))
        {
          request.AddBody(bodyInput);
        }

        // Handle response data from API
        var ress = client.Execute(request);
        if (ress != null)
        {
          if (ress.StatusCode == HttpStatusCode.OK)
          {
            if (!string.IsNullOrWhiteSpace(ress.Content))
            {
              result = JsonConvert.DeserializeObject<ApiResponseGoShip<T>>(ress.Content);
              if (result?.Code != HttpStatusCode.OK)
              {
                //NLogger.log.Error("ApiResponseGoShip.Code:{0} | ApiResponseGoShip.Message:{1}", result.Code, result.Message);
                //NLogger.log.Error("Url:{0} | Method:{1} | Paramters:{2}", strURL, method, JsonConvert.SerializeObject(_params));
                //if (!string.IsNullOrWhiteSpace(bodyInput)) NLogger.log.Error("BodyInput: {0}", JsonConvert.SerializeObject(bodyInput));

                result = new ApiResponseGoShip<T>(result.Code, "", default(T));
              }
            }
          }
          else
          {
            //NLogger.log.Error("HttpStatusCode:{0} | ResponseStatus:{1} | ErrorMsg:{2}", ress.StatusCode, ress.ResponseStatus, ress.ErrorException?.ToString());
            //NLogger.log.Error("Url:{0} | Method:{1} | Paramters:{2}", strURL, method, JsonConvert.SerializeObject(_params));
            //if (!string.IsNullOrWhiteSpace(bodyInput)) NLogger.log.Error("BodyInput: {0}", JsonConvert.SerializeObject(bodyInput));
            // kiểm tra xem có phải hết hạn ko?
            result = new ApiResponseGoShip<T>(HttpStatusCode.BadRequest, "", default(T));
          }
        }
        else
        {
          //NLogger.log.Error("Response from API is null | Url:{0} | Method:{1} | Paramters:{2}", strURL, method, JsonConvert.SerializeObject(_params));
          //if (!string.IsNullOrWhiteSpace(bodyInput)) NLogger.log.Error("BodyInput: {0}", JsonConvert.SerializeObject(bodyInput));

          result = new ApiResponseGoShip<T>(HttpStatusCode.NoContent, "Response from API is null", default(T));
        }
      }
      catch (Exception ex)
      {
        result = new ApiResponseGoShip<T>(HttpStatusCode.ExpectationFailed, ex.ToString(), default(T));
        //NLogger.log.Error(ex, "Error Exception while call PCS_API");
        //NLogger.log.Error("Url:{0} | Method:{1} | Paramters:{2}", strURL, method, JsonConvert.SerializeObject(_params));
        //if (!string.IsNullOrWhiteSpace(bodyInput))
        //    NLogger.log.Error("BodyInput: {0}", JsonConvert.SerializeObject(bodyInput));
      }

      return result;
    }

  }
}
