using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Service.IService;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;

namespace ICHI_API.Service
{
  public class VnPayService : IVnPayService
  {
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public VnPayService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {

      _configuration = configuration;
      _unitOfWork = unitOfWork;
    }

    public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
    {
      var tick = DateTime.Now.Ticks.ToString();
      var vnpay = new VnPayLibrary();

      vnpay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
      vnpay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
      vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
      vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

      vnpay.AddRequestData("vnp_CreateDate", model.CreateDate.ToString("yyyyMMddHHmmss"));
      vnpay.AddRequestData("vnp_CurrCode", "VND");
      vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
      vnpay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
      vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + model.TrxTransactionId);
      vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
      vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]);

      vnpay.AddRequestData("vnp_TxnRef", model.TrxTransactionId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                                                                             //Add Params of 2.1.0 Version

      var paymentUrl = vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
      return paymentUrl;
    }

    public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
      var vnpay = new VnPayLibrary();
      foreach (var (key, value) in collections)
      {
        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
        {
          vnpay.AddResponseData(key, value.ToString());
        }
      }
      var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
      var vnp_transactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
      var vnp_SecureHash = collections.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
      var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
      var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

      bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["Vnpay:HashSecret"]);

      if (!checkSignature)
      {
        return new VnPaymentResponseModel
        {
          Success = false,
        };
      }
      return new VnPaymentResponseModel
      {
        Success = true,
        PaymentMethod = "VnPay",
        OrderDescription = vnp_OrderInfo,
        OrderId = vnp_orderId.ToString(),
        PaymentId = vnp_transactionId.ToString(),
        TransactionId = vnp_transactionId.ToString(),
        Token = vnp_transactionId.ToString(),
        VnPayResponseCode = vnp_ResponseCode
      };
    }

    public VnPaymentResponseModel PaymentCallBack(HttpRequest request, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var respon = PaymentExecute(request.Query);
        if (respon == null || respon.VnPayResponseCode != "00")
        {
          strMessage = "Thanh toán không thành công";
          return respon;
        }
        // cập nhật trạng thái thanh toán của đơn hàng
        var trxTransaction = _unitOfWork.TrxTransaction.Get(u => u.Id == Convert.ToInt32(respon.OrderId));
        if (trxTransaction == null)
        {
          strMessage = "Không tìm thấy đơn hàng";
          return respon;
        }
        trxTransaction.PaymentStatus = AppSettings.PaymentStatusApproved;
        trxTransaction.SessionId = respon.PaymentId;

        trxTransaction.PaymentDate = DateTime.Now;
        _unitOfWork.TrxTransaction.Update(trxTransaction);
        _unitOfWork.Save();

        strMessage = "Thanh toán thành công";
        return respon;
      }
      catch (Exception ex)
      {
        strMessage = "Có lỗi xảy ra";
        NLogger.log.Error(ex.ToString());
        return null;
      }

    }
  }
}
