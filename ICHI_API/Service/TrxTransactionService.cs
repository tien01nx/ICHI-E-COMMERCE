using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
    public class TrxTransactionService : ITrxTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;

        public TrxTransactionService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _db = pcsApiContext;
        }

        public TrxTransactionDTO InsertTxTransaction(TrxTransactionDTO trxTransactionDTO, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var cart = _unitOfWork.Cart.GetAll(u => u.UserId == trxTransactionDTO.UserId);
                TrxTransaction trxTransaction = new TrxTransaction();
                trxTransaction.UserId = trxTransactionDTO.UserId;
                trxTransaction.FullName = trxTransactionDTO.FullName;
                trxTransaction.PhoneNumber = trxTransactionDTO.PhoneNumber;
                trxTransaction.Address = trxTransactionDTO.Address;
                trxTransaction.OrderDate = DateTime.Now;
                trxTransaction.OrderStatus = AppSettings.StatusApproved;
                trxTransaction.PaymentStatus = AppSettings.PaymentStatusPending;
                trxTransaction.OrderTotal = cart.Sum(u => u.Price * u.Quantity);
                _unitOfWork.TrxTransaction.Add(trxTransaction);
                _unitOfWork.Save();
                trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
                trxTransactionDTO.Amount = trxTransaction.OrderTotal;
                // lấy thông tin đơn hàng theo userid từ cart
                foreach (var item in cart)
                {
                    TransactionDetail trxTransactionDetail = new TransactionDetail();
                    trxTransactionDetail.ProductId = item.ProductId;
                    trxTransactionDetail.Total = item.Quantity;
                    trxTransactionDetail.Price = item.Price;
                    trxTransactionDetail.TrxTransactionId = trxTransaction.Id;
                    _unitOfWork.TransactionDetail.Add(trxTransactionDetail);
                    _unitOfWork.Save();
                }
                return trxTransactionDTO;
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                NLogger.log.Error(ex.ToString());
                return null;
            }
        }
    }
}