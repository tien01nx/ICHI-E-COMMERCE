namespace ICHI_API.Service
{
    using System.Linq.Dynamic.Core;
    using ICHI.DataAccess.Repository.IRepository;
    using ICHI_API.Data;
    using ICHI_API.Model;
    using ICHI_API.Service.IService;
    using ICHI_CORE.Domain.MasterModel;
    using ICHI_CORE.Helpers;
    using ICHI_CORE.Model;
    using ICHI_CORE.NlogConfig;
    using Microsoft.AspNetCore.Hosting;

    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = pcsApiContext;
        }

        public Helpers.PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.Product.GetAll(u => u.isDeleted == false, "Category,Trademark").AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.ProductName.Contains(name));
                }

                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);

                // Project query results to ProductDTO
                var pagedResult = Helpers.PagedResult<ProductDTO>.CreatePagedResult(query.Select(p => new ProductDTO
                { Product = p, ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == p.Id, null), CategoryProduct = p.Category }), pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public ProductDTO FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                ProductDTO productDTO = new ProductDTO
                {
                    Product = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted == false),
                    ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == id).ToList(),
                    CategoryProduct = _unitOfWork.Category.Get(u => u.Id == id)
                };
                if (productDTO == null)
                {
                    strMessage = "Sản phẩm không tồn tại";
                    return null;
                }
                else
                {
                    return productDTO;
                }
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }

        public Product Create(Product product, List<IFormFile>? files, out string strMessage)
        {
            strMessage = string.Empty;
            _unitOfWork.BeginTransaction(); // Bắt đầu giao dịch

            try
            {
                if (product.Id == 0)
                {
                    var checkProduct = _unitOfWork.Product.Get(x => x.ProductName == product.ProductName);
                    if (checkProduct != null)
                    {
                        strMessage = "Sản phẩm đã tồn tại";
                        return null;
                    }

                    product.CreateBy = "Admin";
                    product.ModifiedBy = "Admin";

                    _unitOfWork.Product.Add(product);
                    _unitOfWork.Save();

                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            var image = new ProductImages();
                            image.ProductId = product.Id;
                            image.ImageName = file.FileName;
                            image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id, file, AppSettings.PatchProduct);
                            image.IsDefault = false;
                            image.IsActive = true;
                            image.IsDeleted = false;
                            image.CreateBy = "Admin";
                            image.ModifiedBy = "Admin";
                            _unitOfWork.ProductImages.Add(image);
                        }
                        _unitOfWork.Save();
                    }

                    strMessage = "Tạo mới thành công";
                    _unitOfWork.Commit(); // Commit giao dịch
                    return product;
                }

                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                if (files.Count > 0)
                {
                    var productImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == product.Id);

                    foreach (var item in productImages)
                    {
                        ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, item.ImagePath);
                        _unitOfWork.ProductImages.Remove(item);
                    }
                }

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (!ImageHelper.CheckImage(file))
                        {
                            strMessage = "File không đúng định dạng hoặc dung lượng lớn hơn 10MB";
                            _unitOfWork.Rollback();
                            return null;
                        }

                        var image = new ProductImages();
                        image.ProductId = product.Id;
                        image.ImageName = file.FileName;
                        image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id, file, AppSettings.PatchProduct);
                        image.IsDefault = false;
                        image.IsActive = true;
                        image.IsDeleted = false;
                        image.CreateBy = "Admin";
                        image.ModifiedBy = "Admin";
                        _unitOfWork.ProductImages.Add(image);
                    }
                }

                _unitOfWork.Save();
                strMessage = "Cập nhật sản phẩm thành công";
                _unitOfWork.Commit();
                return product;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                _unitOfWork.Rollback();
                return null;
            }
        }

        public Product Update(Product customer, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                // kiêm tra mã số thueé
                customer.ModifiedBy = "Admin";
                _unitOfWork.Product.Update(customer);
                _unitOfWork.Save();
                strMessage = "Cập nhật thành công";
                return customer;
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
                var data = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted);
                if (data == null)
                {
                    strMessage = "khách hàng không tồn tại";
                    return false;
                }

                data.isDeleted = true;
                data.ModifiedDate = DateTime.Now;
                _unitOfWork.Product.Update(data);
                _unitOfWork.Save();
                strMessage = "Xóa thành công";
                return true;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return false;
            }
        }

        public bool DeleteProductImage(int productId, string imageName, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var productImage = _unitOfWork.ProductImages.Get(x => x.ProductId == productId && x.ImageName == imageName);
                if (productImage == null)
                {
                    strMessage = "Hình ảnh sản phẩm không tồn tại!";
                    return false;
                }

                if (!ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, productImage.ImagePath))
                {
                    strMessage = "Xóa ảnh không thành công!";
                    return false;
                }

                _unitOfWork.ProductImages.Remove(productImage);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = "Có lỗi xảy ra";
                return false;
            }

            throw new NotImplementedException();
        }

    }
}
