using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;
using static ICHI_API.Helpers.Constants;

namespace ICHI_API.Service
{
    public class ProductReturnService : IProductReturnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;
        private readonly IAuthService _authService;

        public ProductReturnService(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _db = pcsApiContext;
            _authService = authService;
        }

        public Helpers.PagedResult<ProductReturn> GetAll(string name, string status, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.ProductReturn.GetAll(includeProperties: "Employee,TrxTransaction").AsQueryable();
                foreach (var item in query)
                {
                    item.TrxTransaction = _unitOfWork.TrxTransaction.Get(u => u.Id == item.TrxTransactionId, includeProperties: "Customer");
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.TrxTransaction.Customer.FullName.Contains(name.Trim()));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(e => e.Status == status);
                }
                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);
                var pagedResult = Helpers.PagedResult<ProductReturn>.CreatePagedResult(query, pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductReturnVM FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                ProductReturnVM productReturnVM = new ProductReturnVM();
                var productReturn = _unitOfWork.ProductReturn.Get(u => u.Id == id, includeProperties: "Employee,TrxTransaction");
                if (productReturn == null)
                {
                    throw new BadRequestException("Phiếu đổi trả không tồn tại");
                }
                productReturnVM.ProductReturn = productReturn;
                productReturn.TrxTransaction.Customer = _unitOfWork.Customer.Get(u => u.Id == productReturn.TrxTransaction.CustomerId);
                productReturnVM.ProductReturnDetails = _unitOfWork.ProductReturnDetail.GetAll(u => u.ProductReturnId == id, includeProperties: "Product").ToList();
                foreach (var item in productReturnVM.ProductReturnDetails)
                {
                    item.Product.Image = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
                }

                return productReturnVM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductReturnDTO Create(ProductReturnDTO model, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                _unitOfWork.BeginTransaction();
                //var checkProductReturn = _unitOfWork.ProductReturn.Get(u => u.TrxTransactionId == model.TrxTransactionId);
                //if (checkProductReturn != null)
                //{
                //  throw new BadRequestException(PRODUCTRETURNEXIST);
                //}

                ProductReturn productReturn = new ProductReturn();

                productReturn.TrxTransactionId = model.TrxTransactionId;
                productReturn.EmployeeId = _unitOfWork.Employee.Get(u => u.UserId == _authService.GetUserEmail()).Id;
                productReturn.Status = model.Status;
                productReturn.Reason = model.Reason;
                productReturn.CreateBy = "Admin";
                productReturn.ModifiedBy = "Admin";
                productReturn.CreateDate = DateTime.Now;
                productReturn.ModifiedDate = DateTime.Now;
                _unitOfWork.ProductReturn.Add(productReturn);
                _unitOfWork.Save();
                foreach (var item in model.ReturnProductDetails)
                {
                    ProductReturnDetail productReturnDetail = new ProductReturnDetail();
                    productReturnDetail.ProductReturnId = productReturn.Id;
                    productReturnDetail.ProductId = item.ProductId;
                    productReturnDetail.Price = item.Price;
                    productReturnDetail.Quantity = item.Quantity;
                    productReturnDetail.Reason = item.Reason;
                    productReturnDetail.ReturnType = item.ReturnType;
                    productReturnDetail.CreateBy = "Admin";
                    productReturnDetail.ModifiedBy = "Admin";
                    _unitOfWork.ProductReturnDetail.Add(productReturnDetail);
                }
                _unitOfWork.Save();
                strMessage = ADDPROMOTIONSUCCESS;
                _unitOfWork.Commit();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductReturnDTO Update(ProductReturnDTO model, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                //_unitOfWork.BeginTransaction();
                //// lấy thông tin chương trình khuyến mãi
                //var data = _unitOfWork.ProductReturn.Get(u => u.Id == model.Id);
                //if (data == null)
                //{
                //  throw new BadRequestException(PROMOTIONNOTFOUNDID);
                //}
                //// kiểm tra xem mã chương trình khuyến mãi đã tồn tại chưa
                //var checkProductReturn = _unitOfWork.ProductReturn.Get(u => u.ProductReturnName == model.ProductReturnName);
                //if (checkProductReturn != null && checkProductReturn.Id != model.Id)
                //{
                //  throw new BadRequestException(PROMOTIONEXISTNAME);
                //}
                //ProductReturn promotion = new ProductReturn();
                //promotion.Id = model.Id;
                //promotion.ProductReturnName = model.ProductReturnName;
                //promotion.Discount = model.Discount;
                //promotion.StartTime = model.StartTime;
                //promotion.EndTime = model.EndTime;
                //promotion.isActive = model.isActive;
                //promotion.isDeleted = model.isDeleted;
                //promotion.ModifiedBy = "Admin";
                //_unitOfWork.ProductReturn.Update(promotion);
                //_unitOfWork.Save();
                //var promotionDetails = _unitOfWork.ProductReturnDetail.GetAll(u => u.ProductReturnId == model.Id, "Product").AsQueryable();
                //var promotionDetailDelete = promotionDetails.Where(x => !model.ProductReturnDetails.Select(y => y.Id).Contains(x.Id)).ToList();
                //var promotionDetailUpdate = promotionDetails.Where(x => !model.ProductReturnDetails.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
                //var promotionDetailNew = model.ProductReturnDetails.Where(x => x.Id == 0).ToList();
                //_unitOfWork.ProductReturnDetail.RemoveRange(promotionDetailDelete);
                //List<int> existingProductIds = new List<int>();
                //// thực hiện tạo mới product thêm vào chi tiết chương trình khuyến mãi
                //if (model.ProductReturnDetails != null && promotionDetailNew.Count > 0)
                //{
                //  foreach (var itemAdd in promotionDetailNew)
                //  {
                //    var productProductReturnDetail = _unitOfWork.ProductReturnDetail.GetAll(u => u.ProductId == itemAdd.ProductId, "ProductReturn").AsQueryable();
                //    if (CheckProductProductReturn(productProductReturnDetail, model.StartTime, model.EndTime) == false)
                //    {
                //      existingProductIds.Add(itemAdd.ProductId);
                //      continue;
                //    }

                //    itemAdd.ProductReturnId = model.Id;
                //    itemAdd.ProductId = itemAdd.ProductId;
                //    itemAdd.Quantity = itemAdd.Quantity;
                //    itemAdd.UsedCodesCount = itemAdd.UsedCodesCount;
                //    itemAdd.CreateBy = "Admin";
                //    itemAdd.ModifiedBy = "Admin";
                //    _unitOfWork.ProductReturnDetail.Add(itemAdd);
                //  }
                //}

                //// update theo danh sách sản phẩm
                //if (promotionDetailUpdate.Count > 0)
                //{
                //  foreach (var itemUpdate in promotionDetailUpdate)
                //  {
                //    var newProductId = model.ProductReturnDetails.FirstOrDefault(x => x.ProductId != itemUpdate.ProductId)?.ProductId;

                //    if (newProductId != null)
                //    {

                //      var productProductReturnDetail = _unitOfWork.ProductReturnDetail.GetAll(u => u.ProductId == newProductId, "ProductReturn").AsQueryable();
                //      if (CheckProductProductReturn(productProductReturnDetail, model.StartTime, model.EndTime) == false)
                //      {
                //        existingProductIds.Add(newProductId.Value);
                //        continue;
                //      }
                //      itemUpdate.ProductId = newProductId.Value;
                //      itemUpdate.Quantity = model.ProductReturnDetails.FirstOrDefault(x => x.ProductId == newProductId.Value).Quantity;
                //      itemUpdate.UsedCodesCount = model.ProductReturnDetails.FirstOrDefault(x => x.ProductId == newProductId.Value).UsedCodesCount;
                //      itemUpdate.ModifiedBy = "Admin";
                //      itemUpdate.ModifiedDate = DateTime.Now;
                //    }
                //  }
                //}

                //if (existingProductIds.Any())
                //{
                //  throw new BadRequestException(PROMOTIONEXISTPRODUCT(existingProductIds));
                //}
                //strMessage = UPDATEPROMOTIONSUCCESS;
                //_unitOfWork.Save();
                //_unitOfWork.Commit();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
