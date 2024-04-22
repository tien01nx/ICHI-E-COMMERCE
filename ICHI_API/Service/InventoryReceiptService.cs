using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Globalization;
using System.Linq.Dynamic.Core;
using static ICHI_API.Helpers.Constants;

namespace ICHI_API.Service
{
    public class InventoryReceiptService : IInventoryReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private PcsApiContext _db;

        public InventoryReceiptService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _db = pcsApiContext;
        }

        public InventoryReceipt Create(InventoryReceiptDTO data, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                // lấy ra employeeId theo userId 
                var employee = _unitOfWork.Employee.Get(u => u.UserId == data.EmployeeId);
                InventoryReceipt model = new InventoryReceipt
                {
                    SupplierId = data.SupplierId,
                    EmployeeId = employee.Id,
                    Notes = data.Notes,
                };
                _unitOfWork.InventoryReceipt.Add(model);
                _unitOfWork.Save();
                // lấy ra danh sách productId trong product
                var product = _unitOfWork.Product.GetAll();
                foreach (var item in data.InventoryReceiptDetails)
                {
                    var productItem = product.FirstOrDefault(u => u.Id == item.ProductId);
                    if (productItem == null)
                    {
                        throw new BadRequestException(PRODUCTNOTFOUNDINVENTORY);
                    }
                    item.InventoryReceiptId = model.Id;
                    item.BatchNumber = item.BatchNumber;
                    item.ProductId = item.ProductId;
                    item.Total = item.Total;
                    item.Price = item.Price;
                    _unitOfWork.InventoryReceiptDetail.Add(item);
                }
                _unitOfWork.Save();
                strMessage = CREATEINVENTORYSUCCESS;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public InventoryReceipt Update(InventoryReceiptDTO data, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                _unitOfWork.BeginTransaction();
                var inventory = _unitOfWork.InventoryReceipt.Get(u => u.Id == data.Id);
                if (inventory == null)
                {
                    throw new BadRequestException(INVENTORYNOTFOUND);
                }
                var user = _unitOfWork.Employee.Get(u => u.UserId == data.EmployeeId);
                inventory.SupplierId = data.SupplierId;
                inventory.EmployeeId = user.Id;
                inventory.isActive = true;
                inventory.Notes = data.Notes;
                _unitOfWork.InventoryReceipt.Update(inventory);
                // lấy ra danh sách productId trong product
                //_unitOfWork.InventoryReceiptDetail.RemoveRange(_unitOfWork.InventoryReceiptDetail.GetAll(u => u.InventoryReceiptId == data.Id));
                var product = _unitOfWork.Product.GetAll();
                foreach (var item in data.InventoryReceiptDetails)
                {
                    var productItem = product.FirstOrDefault(u => u.Id == item.ProductId);
                    if (productItem == null)
                    {
                        throw new BadRequestException(PRODUCTNOTFOUNDINVENTORY);
                    }
                    item.InventoryReceiptId = data.Id;
                    item.BatchNumber = item.BatchNumber;
                    item.ProductId = item.ProductId;
                    item.Total = item.Total;
                    item.Price = item.Price;
                    _unitOfWork.InventoryReceiptDetail.Add(item);
                    productItem.Quantity += item.Total;
                    _unitOfWork.Product.Update(productItem);
                }

                _unitOfWork.Save();
                _unitOfWork.Commit();
                strMessage = UPDATEINVENTORYSUCCESS;
                return inventory;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool Delete(int id, out string strMessage)
        {
            throw new NotImplementedException();
        }

        public InventoryReceiptDTO FindById(int id, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var data = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.InventoryReceiptId == id, includeProperties: "InventoryReceipt,Product").ToList();
                if (data == null)
                {
                    throw new BadRequestException(PRODUCTNOTFOUNDINVENTORY);
                }
                var employee = _unitOfWork.Employee.Get(u => u.Id == data.FirstOrDefault().InventoryReceipt.EmployeeId);
                var supllier = _unitOfWork.Supplier.Get(u => u.Id == data.FirstOrDefault().InventoryReceipt.SupplierId);
                InventoryReceiptDTO model = new InventoryReceiptDTO
                {
                    Id = data.FirstOrDefault().InventoryReceipt.Id,
                    EmployeeId = data.FirstOrDefault().InventoryReceipt.EmployeeId.ToString(),
                    FullName = employee.FullName,
                    SupplierId = data.FirstOrDefault().InventoryReceipt.SupplierId,
                    SupplierName = supllier.SupplierName,
                    Notes = data.FirstOrDefault().InventoryReceipt.Notes,
                    InventoryReceiptDetails = data,
                    isActive = data.FirstOrDefault().InventoryReceipt.isActive
                };
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Helpers.PagedResult<InventoryReceipt> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.InventoryReceipt.GetAll(includeProperties: "Supplier,Employee").AsQueryable();

                var trimmedName = name.Trim();

                // Kiểm tra xem chuỗi tên có phải là ngày không
                if (DateTime.TryParseExact(trimmedName, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    // Nếu là ngày, thực hiện tìm kiếm theo ngày tạo
                    query = query.Where(e => e.CreateDate.Date == DateTime.ParseExact(trimmedName, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);
                }
                else
                {
                    // Nếu không phải là ngày, thực hiện tìm kiếm theo tên của nhà cung cấp
                    query = query.Where(e => e.Supplier.SupplierName.Contains(trimmedName));
                }
                //var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                var orderBy = sortBy switch
                {
                    "FullName" => $"Employee.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
                    "SupplierName" => $"Supplier.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
                    _ => $"Id {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}"
                };

                query = query.OrderBy(orderBy);
                var pagedResult = Helpers.PagedResult<InventoryReceipt>.CreatePagedResult(query, pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // lấy danh sách product và thêm số lô vào product
        public List<Product> GetProductWithBatchNumber(out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var products = _unitOfWork.Product.GetAll().ToList();
                foreach (var item in products)
                {
                    var inventoryReceiptDetail = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.ProductId == item.Id).OrderByDescending(u => u.BatchNumber).FirstOrDefault();
                    item.BatchNumber = inventoryReceiptDetail?.BatchNumber ?? 1;
                }
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public double IncrementVersion(double version)
        //{
        //  if (version <= 1.19)
        //  {
        //    return version += 0.01;
        //  }
        //  else
        //  {
        //    return version = Math.Ceiling(version) + 0.0;
        //  }
        //}
    }
}
