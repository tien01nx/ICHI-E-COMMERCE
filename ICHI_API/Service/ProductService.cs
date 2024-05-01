namespace ICHI_API.Service
{
    using ICHI.DataAccess.Repository.IRepository;
    using ICHI_API.Data;
    using ICHI_API.Migrations;
    using ICHI_API.Model;
    using ICHI_API.Service.IService;
    using ICHI_CORE.Domain.MasterModel;
    using ICHI_CORE.Helpers;
    using iText.StyledXmlParser.Jsoup.Select;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using System.Data;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Text.RegularExpressions;
    using static ICHI_API.Helpers.Constants;
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryProductService _categoryProductService;
        private readonly IPromotionService _promotionService;


        public ProductService(IUnitOfWork unitOfWork, IPromotionService promotionService, IWebHostEnvironment webHostEnvironment, PcsApiContext pcsApiContext, ICategoryProductService categoryProductService)
        {
            _db = pcsApiContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _categoryProductService = categoryProductService;
            _promotionService = promotionService;
        }
        public Helpers.PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.Product.GetAll(u => u.isDeleted == false, "Category,Trademark").AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.ProductName.Contains(name.Trim()));
                }

                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);
                var promotion = _promotionService.CheckPromotionActive();
                foreach (var item in query)
                {
                    //item.Discount = _unitOfWork.PromotionDetail.Get(u => u.ProductId == item.Id, "Promotion")?.Promotion?.Discount ?? 0;
                    item.Discount = promotion.Where(u => u.ProductId == item.Id).FirstOrDefault()?.Promotion?.Discount ?? 0;
                    item.Image += _unitOfWork.ProductImages.GetAll(u => u.ProductId == item.Id).FirstOrDefault()?.ImagePath;
                }

                var pagedResult = Helpers.PagedResult<ProductDTO>.CreatePagedResult(query.Select(p => new ProductDTO
                {
                    Product = p,
                    ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == p.Id, null),
                    CategoryProduct = p.Category,
                }), pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ProductDTO> GetAll()
        {
            List<ProductDTO> productDTO = new List<ProductDTO>();
            try
            {
                var data = _unitOfWork.Product.GetAll(u => u.isDeleted == false, "Category,Trademark").ToList();
                foreach (var item in data)
                {
                    productDTO.Add(new ProductDTO
                    {
                        Product = item,
                        ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == item.Id).ToList(),
                        CategoryProduct = item.Category,
                    });
                }
                return productDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ProductDTO FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var promotion = _promotionService.CheckPromotionActive();
                ProductDTO productDTO = new ProductDTO
                {
                    Product = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted == false),
                    ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == id).ToList(),
                    CategoryProduct = _unitOfWork.Category.Get(u => u.Id == id),
                    PromotionDetail = promotion.Where(u => u.ProductId == id).FirstOrDefault()

                };
                if (productDTO == null)
                {
                    throw new BadRequestException(PRODUCTNOTFOUND);
                }
                else
                {
                    return productDTO;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Product Create(Product product, List<IFormFile>? files, out string strMessage)
        {
            strMessage = string.Empty;
            _unitOfWork.BeginTransaction();
            try
            {
                if (product.Id == 0)
                {
                    if (_unitOfWork.Product.ExistsBy(x => x.ProductName == product.ProductName))
                        throw new BadRequestException(PRODUCTEXIST);
                    product.CreateBy = "Admin";
                    product.ModifiedBy = "Admin";

                    _unitOfWork.Product.Add(product);
                    _unitOfWork.Save();

                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            if (!ImageHelper.CheckImage(file))
                            {
                                _unitOfWork.Rollback();
                                throw new BadRequestException(FILEFORMAT);
                            }
                            var image = new ProductImages();
                            image.ProductId = product.Id;
                            image.ImageName = file.FileName;
                            image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
                            image.IsDefault = false;
                            image.IsActive = true;
                            image.IsDeleted = false;
                            image.CreateBy = "Admin";
                            image.ModifiedBy = "Admin";
                            _unitOfWork.ProductImages.Add(image);
                        }
                    }
                    strMessage = ADDPRODUCTSUCCESS;
                }
                else
                {
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
                                throw new BadRequestException(FILEFORMAT);
                            }
                            var image = new ProductImages();
                            image.ProductId = product.Id;
                            image.ImageName = file.FileName;
                            image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
                            image.IsDefault = false;
                            image.IsActive = true;
                            image.IsDeleted = false;
                            image.CreateBy = "Admin";
                            image.ModifiedBy = "Admin";
                            _unitOfWork.ProductImages.Add(image);
                        }
                    }
                    strMessage = UPDATEPRODUCTSUCCESS;
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Delete(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var data = _unitOfWork.Product.Get(u => u.Id == id && !u.isDeleted);
                if (data == null)
                {
                    throw new BadRequestException(PRODUCTNOTFOUND);
                }

                data.isDeleted = true;
                data.ModifiedDate = DateTime.Now;
                _unitOfWork.Product.Update(data);
                _unitOfWork.Save();
                strMessage = DELETEPRODUCTSUCCESS;
                return true;
            }
            catch (Exception)
            {
                throw;
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
                    throw new BadRequestException(IMAGEPRODUCTNOTFOUND);
                }

                if (!ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, productImage.ImagePath))
                {
                    throw new BadRequestException(DELETEIMAGESUCCESS);
                }

                _unitOfWork.ProductImages.Remove(productImage);
                _unitOfWork.Save();
                strMessage = DELETEIMAGESUCCESS;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Product> FilterProducts(IQueryable<Product> query, string categoryName, string category_parent, string color, string trademark, decimal? priceMin, decimal? priceMax)
        {
            query = query.Where(product => !product.isDeleted);

            if (!string.IsNullOrEmpty(categoryName))
            {
                var data = _unitOfWork.Category.Get(u => u.CategoryName == categoryName);
                if (data == null)
                {
                    return query;
                }
                int categoryId = data.Id;
                //if (categoryId == 0)
                //{
                //    return query;
                //}
                var categories = _categoryProductService.GetCategories(categoryId);
                List<int> lstID = categories.Select(x => x.Id).ToList();
                query = query.Where(x => lstID.Any(id => id == x.CategoryId));
            }

            if (category_parent != null)
            {
                string[] categoryNamekArray = category_parent.Split(',');
                List<string> categories = new List<string>(categoryNamekArray);
                query = query.Where(product => categories.Contains(product.Category.CategoryName));
            }

            if (trademark != null)
            {
                string[] trademarkArray = trademark.Split(',');
                List<string> trademarks = new List<string>(trademarkArray);
                query = query.Where(product => trademarks.Contains(product.Trademark.TrademarkName));
            }

            if (color != null)
            {
                string[] colorArray = color.Split(',');
                List<string> colors = new List<string>(colorArray);
                query = query.Where(product => colors.Contains(product.Color));
            }

            if (priceMin.HasValue)
                query = query.Where(product => product.Price >= priceMin.Value);

            if (priceMax.HasValue)
                query = query.Where(product => product.Price <= priceMax.Value);

            return query;
        }
        public Helpers.PagedResult<ProductDTO> GetProductInCategory(string categoryName, string? category_parent, string? color, string? trademarkName, decimal? priceMin, decimal? priceMax, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.Product.GetAll(includeProperties: "Category,Trademark").AsQueryable();
                query = FilterProducts(query, categoryName, category_parent, color, trademarkName, priceMin, priceMax);
                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                query = query.OrderBy(orderBy);

                var pagedResult = Helpers.PagedResult<ProductDTO>.CreatePagedResult(query.Select(p => new ProductDTO
                { Product = p, ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == p.Id, null), CategoryProduct = p.Category }), pageNumber, pageSize);
                return pagedResult;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // top 5 sản phẩm bán chạy nhất tháng data truyền vào là DateTime
        public List<ProductDTO> ProductTopFive(string strDateTime, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                DateTime dateTime = DateTime.Parse(strDateTime);
                int month = dateTime.Month;
                int year = dateTime.Year;
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                var data = _unitOfWork.TransactionDetail.GetAll(u => u.CreateDate.Year == year && u.CreateDate.Month == month, "TrxTransaction").GroupBy(u => u.ProductId).OrderByDescending(g => g.Sum(u => u.Quantity)).Take(5);
                var productId = data.Select(g => g.Key).Take(5);
                foreach (var item in productId)
                {
                    productDTOs.Add(new ProductDTO
                    {
                        Product = _unitOfWork.Product.Get(u => u.Id == item && u.isDeleted == false),
                        ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == item).ToList(),
                        CategoryProduct = _unitOfWork.Category.Get(u => u.Id == item),
                        // số lượng đã giao thành công
                        quantitySold = data.SelectMany(g => g).Where(u => u.TrxTransaction.OrderStatus == AppSettings.StatusOrderDelivered && u.TrxTransaction.PaymentStatus == AppSettings.PaymentStatusApproved && u.ProductId == item).Sum(u => u.Quantity),
                        // số lượng đang giao đến người dùng
                        QuantityOnnTheOrder = data.SelectMany(g => g).Where(u => u.TrxTransaction.OrderStatus != AppSettings.StatusOrderDelivered && u.TrxTransaction.OrderStatus != AppSettings.StatusOrderCancelled && u.ProductId == item).Sum(u => u.Quantity)
                    });
                }
                return productDTOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProductDTO> Search(string productName, out string strMessage)
        {
            try
            {
                strMessage = string.Empty;
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                var data = _unitOfWork.Product.GetAll(u => u.ProductName.Contains(productName) && u.isDeleted == false, "Category,Trademark").ToList();
                foreach (var item in data)
                {
                    productDTOs.Add(new ProductDTO
                    {
                        Product = item,
                        ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == item.Id).ToList(),
                        CategoryProduct = item.Category,
                    });
                }
                return productDTOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
