using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
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

        public Helpers.PagedResult<TrxTransaction> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _db.TrxTransactions.AsQueryable();
                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);
                var pagedResult = Helpers.PagedResult<TrxTransaction>.CreatePagedResult(query, pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public TrxTransaction FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var data = _unitOfWork.TrxTransaction.Get(u => u.Id == id);
                if (data == null)
                {
                    strMessage = "Nhà cung cấp không tồn tại";
                    return null;
                }
                return data;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public TrxTransaction Create(TrxTransaction category, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                category.CreateBy = "Admin";
                category.ModifiedBy = "Admin";
                _unitOfWork.TrxTransaction.Add(category);
                _unitOfWork.Save();
                strMessage = "Tạo mới danh mục thành công";
                return category;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public TrxTransaction Update(TrxTransaction category, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                // lấy thông tin nhà cung cấp
                var data = _unitOfWork.TrxTransaction.Get(u => u.Id == category.Id);
                if (data == null)
                {
                    strMessage = "Danh mục sản phẩm không tồn tại";
                    return null;
                }

                category.ModifiedBy = "Admin";
                _unitOfWork.TrxTransaction.Update(category);
                _unitOfWork.Save();
                strMessage = "Cập nhật nhà cung cấp thành công";
                return category;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public bool Delete(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return false;
            }
        }

        public Cart InsertCart(Cart cart, out string strMessage)
        {
            // thục hiện thêm dữ liệu userId và productId vào bảng Cart
            strMessage = string.Empty;

            try
            {
                _unitOfWork.Cart.Add(cart);
                _unitOfWork.Save();
                strMessage = "Thêm sản phẩm vào giỏ hàng thành công";
                return cart;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }
    }
}