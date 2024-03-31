using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;

        public PromotionService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _db = pcsApiContext;
        }

        public Helpers.PagedResult<Promotion> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {

                ////var query = _db.Promotions.AsQueryable().Where(u => u.isDeleted == false);
                var query = _unitOfWork.Promotion.GetAll(u => u.isDeleted == false).AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.PromotionName.Contains(name));
                }
                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);
                var pagedResult = Helpers.PagedResult<Promotion>.CreatePagedResult(query, pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public Promotion FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var data = _unitOfWork.Promotion.Get(u => u.Id == id);
                if (data == null)
                {
                    strMessage = "Mã chương trình khuyến mãi không tồn tại";
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

        public PromotionDTO Create(PromotionDTO model, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                _unitOfWork.BeginTransaction();
                var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionCode == model.Promotion.PromotionCode);
                if (checkPromotion != null)
                {
                    strMessage = "Mã chương trình khuyến mãi đã tồn tại";
                    return null;
                }

                model.Promotion.CreateBy = "Admin";
                model.Promotion.ModifiedBy = "Admin";
                _unitOfWork.Promotion.Add(model.Promotion);
                _unitOfWork.Save();

                // thực hiện thêm chi tiết chương trình khuyến mãi
                if (model.PromotionDetails != null && model.PromotionDetails.Count() > 0)
                {
                    foreach (var item in model.PromotionDetails)
                    {
                        var product = _unitOfWork.Product.Get(u => u.Id == item.ProductId);
                        if (product == null)
                        {
                            strMessage = "Sản phẩm không tồn tại";
                            return null;
                        }
                        // thực hiện kiểm tra sản phẩm đã tồn tại trong chương trình khuyến mãi chưa, đảm bảo mã sản phẩm đó phải hết thời hạn trước  đó thì mới cho thêm
                        var checkPromotionDetail = _unitOfWork.PromotionDetail.Get(u => u.ProductId == item.ProductId && u.PromotionId == model.Promotion.Id
                                                                                && u.Promotion.EndTime < DateTime.Now.AddDays(1).AddSeconds(-1), "Promotion");
                        if (checkPromotionDetail != null)
                        {
                            strMessage = "Sản phẩm đã tồn tại trong chương trình khuyến mãi";
                            return null;
                        }

                        item.PromotionId = model.Promotion.Id;
                        item.ProductId = item.ProductId;
                        item.CreateBy = "Admin";
                        item.ModifiedBy = "Admin";
                        _unitOfWork.PromotionDetail.Add(item);
                    }
                    _unitOfWork.Save();
                }

                strMessage = "Tạo mới thành công";
                _unitOfWork.Commit();
                return model;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public PromotionDTO Update(PromotionDTO model, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {

                // lấy thông tin chương trình khuyến mãi
                var data = _unitOfWork.Promotion.Get(u => u.Id == model.Promotion.Id);
                if (data == null)
                {
                    strMessage = "Mã chương trình khuyến mãi không tồn tại";
                    return null;
                }
                // kiểm tra xem mã chương trình khuyến mãi đã tồn tại chưa
                var checkPromotion = _unitOfWork.Promotion.Get(u => u.PromotionCode == model.Promotion.PromotionCode);
                if (checkPromotion != null && checkPromotion.Id != model.Promotion.Id)
                {
                    strMessage = "Mã chương trình khuyến mãi đã tồn tại";
                    return null;
                }

                model.Promotion.ModifiedBy = "Admin";
                _unitOfWork.Promotion.Update(model.Promotion);
                _unitOfWork.Save();
                // thực hiện cập nhật chi tiết chương trình khuyến mãi
                if (model.PromotionDetails != null && model.PromotionDetails.Count() > 0)
                {
                    foreach (var item in model.PromotionDetails)
                    {
                        // danh sách sản phẩm trong chương trình khuyến mãi trong db là
                        var promotionDetailDB = data.PromotionDetails;
                        // dách sách sản phẩm trong chương trình khuyến mãi mà thực hiện thêm mới là khi có id =0
                        var promotionDetail = model.PromotionDetails.Where(u => u.Id == 0).ToList();
                        // danh sách sản phẩm mà thực hiện xóa là 
                        var promotionDetailDelete = promotionDetailDB.Where(u => !model.PromotionDetails.Select(x => x.Id).Contains(u.Id)).ToList();

                        // thực hiện thêm mới sản phẩm
                        if (promotionDetail != null && promotionDetail.Count() > 0)
                        {
                            foreach (var itemAdd in promotionDetail)
                            {
                                var product = _unitOfWork.Product.Get(u => u.Id == itemAdd.ProductId);
                                if (product == null)
                                {
                                    strMessage = "Sản phẩm không tồn tại";
                                    return null;
                                }
                                // thực hiện kiểm tra sản phẩm đã tồn tại trong chương trình khuyến mãi chưa, đảm bảo mã sản phẩm đó phải hết thời hạn trước  đó thì mới cho thêm
                                var checkPromotionDetail = _unitOfWork.PromotionDetail.Get(u => u.ProductId == itemAdd.ProductId && u.PromotionId == model.Promotion.Id
                                                                                                                       && u.Promotion.EndTime < DateTime.Now.AddDays(1).AddSeconds(-1), "Promotion");
                                if (checkPromotionDetail != null)
                                {
                                    strMessage = "Sản phẩm đã tồn tại trong chương trình khuyến mãi";
                                    return null;
                                }

                                itemAdd.PromotionId = model.Promotion.Id;
                                itemAdd.ProductId = itemAdd.ProductId;
                                itemAdd.CreateBy = "Admin";
                                itemAdd.ModifiedBy = "Admin";
                                _unitOfWork.PromotionDetail.Add(itemAdd);
                            }
                        }

                        // thực hiện xóa sản phẩm
                        _unitOfWork.PromotionDetail.RemoveRange(promotionDetailDelete);

                    }
                    _unitOfWork.Save();
                }


                strMessage = "Cập nhật chương trình khuyến mãi thành công";
                return model;
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
                var data = _unitOfWork.Promotion.Get(u => u.Id == id && !u.isDeleted);
                if (data == null)
                {
                    strMessage = "Mã chương trình khuyến mãi không tồn tại";
                    return false;
                }
                data.isDeleted = true;
                data.ModifiedDate = DateTime.Now;
                _unitOfWork.Promotion.Update(data);
                _unitOfWork.Save();
                strMessage = "Xóa chương trình khuyến mãi thành công";
                return true;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return false;
            }
        }
    }
}
