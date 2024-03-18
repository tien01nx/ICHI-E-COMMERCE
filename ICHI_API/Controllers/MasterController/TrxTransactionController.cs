namespace ICHI_API.Controllers.MasterController
{
    using ICHI_API.Data;
    using ICHI_API.Model;
    using ICHI_API.Service.IService;
    using ICHI_CORE.Domain.MasterModel;
    using ICHI_CORE.Model;
    using ICHI_CORE.NlogConfig;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TrxTransactionController : Controller
    {
        private readonly ITrxTransactionService _trxTransactionService;

        public TrxTransactionController(PcsApiContext context, ITrxTransactionService trxTransactionService, IConfiguration configuration = null)
        {
            _trxTransactionService = trxTransactionService;
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
    }
}
