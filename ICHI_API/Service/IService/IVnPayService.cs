using ICHI_CORE.Model;

namespace ICHI_API.Service.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
        bool PaymentCallBack(HttpRequest request, out string strMessage);
    }
}
