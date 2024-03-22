using ICHI_CORE.Model;

namespace ICHI_API.Service.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
        VnPaymentResponseModel PaymentCallBack(HttpRequest request, out string strMessage);
    }
}
