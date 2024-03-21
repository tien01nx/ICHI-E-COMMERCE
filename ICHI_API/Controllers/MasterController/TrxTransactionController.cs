namespace ICHI_API.Controllers.MasterController
{
    using ICHI_API.Data;
    using ICHI_API.Model;
    using ICHI_API.Service.IService;
    using ICHI_CORE.Domain.MasterModel;
    using ICHI_CORE.Model;
    using ICHI_CORE.NlogConfig;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TrxTransactionController : Controller
    {
        private readonly ITrxTransactionService _trxTransactionService;
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrxTransactionController(PcsApiContext context, ITrxTransactionService trxTransactionService, IVnPayService vnPayService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration = null)
        {
            _trxTransactionService = trxTransactionService;
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
        }

        [HttpPost("InsertTxTransaction")]
        public async Task<ApiResponse<TrxTransactionDTO>> InsertTxTransaction([FromBody] TrxTransactionDTO trxTransactionDTO)
        {
            ApiResponse<TrxTransactionDTO> result;
            string strMessage = string.Empty;
            try
            {
                var data = _trxTransactionService.InsertTxTransaction(trxTransactionDTO, out strMessage);
                return new ApiResponse<TrxTransactionDTO>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<TrxTransactionDTO>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpPost("CreatePaymentUrl")]
        public async Task<ApiResponse<string>> CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            ApiResponse<string> result;
            string strMessage = string.Empty;
            try
            {
                var data = _vnPayService.CreatePaymentUrl(HttpContext, model);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpGet("PaymentExecute")]
        public async Task<ApiResponse<VnPaymentResponseModel>> PaymentExecute([FromQuery] IQueryCollection collections)
        {
            ApiResponse<VnPaymentResponseModel> result;
            string strMessage = string.Empty;
            try
            {
                var data = _vnPayService.PaymentExecute(collections);
                return new ApiResponse<VnPaymentResponseModel>(System.Net.HttpStatusCode.OK, strMessage, data);
            }
            catch (Exception ex)
            {
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<VnPaymentResponseModel>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }

        [HttpGet("GetTransaction")]
        public async Task<ApiResponse<string>> GetTransaction()
        {
            ApiResponse<string> result;
            string strMessage = string.Empty;
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                var data = _vnPayService.PaymentCallBack(request, out strMessage);
                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, strMessage, null);
            }
            catch (Exception ex)
            {
                strMessage = "Có lỗi xảy ra";
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<string>(System.Net.HttpStatusCode.ExpectationFailed, strMessage, null);
            }
            return result;
        }


    }
}
