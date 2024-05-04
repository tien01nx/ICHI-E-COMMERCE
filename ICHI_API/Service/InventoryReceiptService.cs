using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using OfficeOpenXml;
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
                    item.Quantity = item.Quantity;
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
                    item.Quantity = item.Quantity;
                    item.Price = item.Price;
                    _unitOfWork.InventoryReceiptDetail.Update(item);
                    productItem.Quantity += item.Quantity;
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
                    item.BatchNumber = IncrementVersion(inventoryReceiptDetail?.BatchNumber ?? 1);
                }
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double IncrementVersion(double version)
        {
            if (version <= 1.19)
            {
                return version += 0.01;
            }
            else
            {
                return version = Math.Ceiling(version) + 0.0;
            }
        }

        // lấy ra danh sách sản phẩm trong hóa đơn nhập 
        // Tháng/ Tổng số lượng / Tổng tiền
        public List<InventoryModel> InventoryModels(int year, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                List<InventoryModel> inventoryModels = new List<InventoryModel>();
                for (int i = 1; i <= 12; i++)
                {
                    var data = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.CreateDate.Month == i && u.CreateDate.Year == year, includeProperties: "InventoryReceipt").ToList();
                    if (data.Count > 0)
                    {
                        InventoryModel model = new InventoryModel
                        {
                            Month = i,
                            TotalQuantity = data.Sum(u => u.Quantity),
                            TotalPrice = data.Sum(u => u.Quantity * u.Price)
                        };
                        inventoryModels.Add(model);
                    }
                }

                return inventoryModels;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public byte[] GenerateExcelReport(int year)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "TemplateHoaDonNhap.xlsx");
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                // Get the worksheets
                var reportSheet = package.Workbook.Worksheets["Hóa Đơn Nhập"];

                var inventory = _unitOfWork.InventoryReceiptDetail.GetAll(includeProperties: "InventoryReceipt,Product");
                inventory = inventory.Where(u => u.InventoryReceipt.CreateDate.Year == year);

                //STT Ngày    Số Lô   Tên Nhà Cung Cấp    Tên sản phẩm Giá Số lượng    Thành Tiền
                decimal inventoryTotal = 0;
                int inventoryRowStart = 4;
                int i = 1;
                foreach (var item in inventory)
                {
                    for (int col = 1; col <= 9; col++)
                    {
                        var cell = reportSheet.Cells[inventoryRowStart, col];
                        cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    reportSheet.Cells[inventoryRowStart, 1].Value = i;
                    reportSheet.Cells[inventoryRowStart, 2].Value = item.Id;
                    reportSheet.Cells[inventoryRowStart, 3].Value = item.CreateDate;
                    reportSheet.Cells[inventoryRowStart, 3].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM"; // Định dạng ngày tháng
                    reportSheet.Cells[inventoryRowStart, 4].Value = item.BatchNumber;
                    reportSheet.Cells[inventoryRowStart, 5].Value = _unitOfWork.Supplier.Get(u => u.Id == item.InventoryReceipt.SupplierId).SupplierName;
                    reportSheet.Cells[inventoryRowStart, 5].Style.Font.Bold = true;
                    reportSheet.Cells[inventoryRowStart, 6].Value = item.Product.ProductName;
                    reportSheet.Cells[inventoryRowStart, 6].Style.Font.Italic = true;
                    reportSheet.Cells[inventoryRowStart, 7].Value = item.Quantity;
                    reportSheet.Cells[inventoryRowStart, 7].Style.Numberformat.Format = "#,##0"; // Định dạng số nguyên cho số lượng
                    reportSheet.Cells[inventoryRowStart, 8].Value = item.Price;
                    reportSheet.Cells[inventoryRowStart, 8].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ cho giá
                    reportSheet.Cells[inventoryRowStart, 9].Formula = $"G{inventoryRowStart}*H{inventoryRowStart}"; // Giá mua * Số lượng
                    reportSheet.Cells[inventoryRowStart, 9].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ
                    inventoryTotal += item.Price * item.Quantity;
                    inventoryRowStart++;
                    i++;
                }
                reportSheet.Cells[inventoryRowStart, 8].Value = "Thành tiền";
                reportSheet.Cells[inventoryRowStart, 9].Value = inventoryTotal;
                reportSheet.Cells[inventoryRowStart, 9].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ
                reportSheet.Cells[inventoryRowStart, 8].Style.Font.Bold = true;
                reportSheet.Cells[inventoryRowStart, 9].Value = inventoryTotal;
                reportSheet.Cells[inventoryRowStart, 9].Style.Font.Bold = true;
                reportSheet.Cells[inventoryRowStart, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                reportSheet.Cells[inventoryRowStart, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(221, 235, 247)); // Màu nền xanh nhạt


                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);

                memoryStream.Position = 0;

                return memoryStream.ToArray();

            }
        }



        //public byte[] GenerateProductExcelReport(int year)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "TemplateHoaDonNhap.xlsx");
        //    var fileInfo = new FileInfo(filePath);

        //    using (var package = new ExcelPackage(fileInfo))
        //    {
        //        // Get the worksheets
        //        var reportSheet = package.Workbook.Worksheets["Hóa Đơn Nhập"];

        //        //var inventory = _unitOfWork.InventoryReceiptDetail.GetAll(includeProperties: "InventoryReceipt,Product");
        //        //inventory = inventory.Where(u => u.InventoryReceipt.CreateDate.Year == year);

        //        var product = _unitOfWork.Product.GetAll(includeProperties: "Trademark,Category");


        //        //STT Mã Sản Phẩm Số Lô   Tên sản phẩm Thương Hiệu Tên Nhà Cung Cấp Giá Bán Số Lượng Hiện Tại Số Lượng Đang Giao Thành Tiền


        //        decimal inventoryTotal = 0;
        //        int inventoryRowStart = 4;
        //        int i = 1;
        //        foreach (var item in product)
        //        {
        //            for (int col = 1; col <= 9; col++)
        //            {
        //                var cell = reportSheet.Cells[inventoryRowStart, col];
        //                cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
        //            }
        //            reportSheet.Cells[inventoryRowStart, 1].Value = i;
        //            reportSheet.Cells[inventoryRowStart, 2].Value = item.Id;
        //            // Tìm ra số lô trong InventoryReceiptDetail theo ProductId
        //            reportSheet.Cells[inventoryRowStart, 3].Value = item.BatchNumber;
        //            //reportSheet.Cells[inventoryRowStart, 3].Value = item.CreateDate;
        //            //reportSheet.Cells[inventoryRowStart, 3].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM"; // Định dạng ngày tháng
        //            //reportSheet.Cells[inventoryRowStart, 4].Value = item.BatchNumber;
        //            //reportSheet.Cells[inventoryRowStart, 5].Value = _unitOfWork.Supplier.Get(u => u.Id == item.InventoryReceipt.SupplierId).SupplierName;
        //            //reportSheet.Cells[inventoryRowStart, 5].Style.Font.Bold = true;
        //            //reportSheet.Cells[inventoryRowStart, 6].Value = item.Product.ProductName;
        //            //reportSheet.Cells[inventoryRowStart, 6].Style.Font.Italic = true;
        //            //reportSheet.Cells[inventoryRowStart, 7].Value = item.Quantity;
        //            //reportSheet.Cells[inventoryRowStart, 7].Style.Numberformat.Format = "#,##0"; // Định dạng số nguyên cho số lượng
        //            //reportSheet.Cells[inventoryRowStart, 8].Value = item.Price;
        //            //reportSheet.Cells[inventoryRowStart, 8].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ cho giá
        //            //reportSheet.Cells[inventoryRowStart, 9].Formula = $"G{inventoryRowStart}*H{inventoryRowStart}"; // Giá mua * Số lượng
        //            //reportSheet.Cells[inventoryRowStart, 9].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ
        //            //inventoryTotal += item.Price * item.Quantity;
        //            inventoryRowStart++;
        //            i++;
        //        }
        //        reportSheet.Cells[inventoryRowStart, 8].Value = "Thành tiền";
        //        reportSheet.Cells[inventoryRowStart, 9].Value = inventoryTotal;
        //        reportSheet.Cells[inventoryRowStart, 9].Style.Numberformat.Format = "#,##0₫"; // Định dạng tiền tệ
        //        reportSheet.Cells[inventoryRowStart, 8].Style.Font.Bold = true;
        //        reportSheet.Cells[inventoryRowStart, 9].Value = inventoryTotal;
        //        reportSheet.Cells[inventoryRowStart, 9].Style.Font.Bold = true;
        //        reportSheet.Cells[inventoryRowStart, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        reportSheet.Cells[inventoryRowStart, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(221, 235, 247)); // Màu nền xanh nhạt


        //        var memoryStream = new MemoryStream();
        //        package.SaveAs(memoryStream);

        //        memoryStream.Position = 0;

        //        return memoryStream.ToArray();

        //    }
        //}
    }
}
