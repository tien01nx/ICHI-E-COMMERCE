using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Report;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.WebSockets;
using System.Text;

namespace ICHI_CORE.Controllers.MasterController
{
    [Route("api/ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WebSocketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                // sinh ra ngẫu nhiên giá trị x,y thay đổi sau 2 giây

                var random = new Random();
                while (websocket.State == WebSocketState.Open)
                {

                    int x = random.Next(1, 100);
                    int y = random.Next(1, 100);
                    var buffer = Encoding.UTF8.GetBytes($"{{ \"x\": {x}, \"y\": {y} }}");
                    Console.WriteLine($"x : {x}, y: {y}");
                    await websocket.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text, true, CancellationToken.None);
                    await Task.Delay(1000);

                }
                await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
            }
        }

        //[HttpGet("Demo")]
        //public IActionResult Demo()
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    var list = new List<Student_Report>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        list.Add(new Student_Report
        //        {
        //            MaHS = "HS00" + (i + 1),
        //            HoHS = "Nguyễn Trọng",
        //            TenHS = "Quân" + (i + 1),
        //            NgaySinh = DateTime.Now.AddYears(20),
        //            GioiTinh = "Nam",
        //            DiaChi = "http://saledam.com/",
        //            TenLOP = "Lập trình",
        //            Diem = 10,
        //            TenMH = "Web asp.net Mvc"
        //        });
        //    }

        //    using (var stream = new MemoryStream())
        //    {
        //        using (var package = new ExcelPackage(stream))
        //        {
        //            ExcelWorksheet Sheet = package.Workbook.Worksheets.Add("Report");
        //            Sheet.Cells["A1"].Value = "Mã học sinh";
        //            Sheet.Cells["B1"].Value = "Tên học sinh";
        //            Sheet.Cells["C1"].Value = "Ngày sinh";
        //            Sheet.Cells["D1"].Value = "Giới tính";
        //            Sheet.Cells["E1"].Value = "Địa chỉ";
        //            int row = 2;// dòng bắt đầu ghi dữ liệu
        //            foreach (var item in list)
        //            {
        //                Sheet.Cells[string.Format("A{0}", row)].Value = item.MaHS;
        //                Sheet.Cells[string.Format("B{0}", row)].Value = item.HoHS + " " + item.TenHS;
        //                Sheet.Cells[string.Format("C{0}", row)].Value = item.NgaySinh.ToString("dd/MM/yyyy");
        //                Sheet.Cells[string.Format("D{0}", row)].Value = item.GioiTinh;
        //                Sheet.Cells[string.Format("E{0}", row)].Value = item.DiaChi;
        //                row++;
        //            }
        //            Sheet.Cells["A:AZ"].AutoFitColumns();
        //            package.Save();
        //        }

        //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        //    }
        //}
        [HttpGet("generateReport")]
        public IActionResult GenerateReport()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "TemplateExcel.xlsx");
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                // Get the worksheets
                var reportSheet = package.Workbook.Worksheets["Báo cáo"];
                var profitSheet = package.Workbook.Worksheets["Lợi nhuận"];

                // Sample data for sales and purchases
                var salesData = new List<dynamic>
        {
            new { Index = 1, Date = DateTime.Now, Lot = "L001", ProductName = "Product 1", SalePrice = 200, PurchasePrice = 150, Quantity = 10 },
            new { Index = 2, Date = DateTime.Now, Lot = "L002", ProductName = "Product 2", SalePrice = 250, PurchasePrice = 200, Quantity = 15 }
        };

                var purchaseData = new List<dynamic>
        {
            new { Index = 1, Date = DateTime.Now, Lot = "L001", ProductName = "Product 1", PurchasePrice = 150, Quantity = 10 },
            new { Index = 2, Date = DateTime.Now, Lot = "L002", ProductName = "Product 2", PurchasePrice = 200, Quantity = 15 }
        };

                // Insert data for sales
                decimal salesTotal = 0;
                int salesRowStart = 5;
                foreach (var item in salesData)
                {
                    reportSheet.Cells[salesRowStart, 1].Value = item.Index;
                    reportSheet.Cells[salesRowStart, 2].Value = item.Date.ToShortDateString();
                    reportSheet.Cells[salesRowStart, 3].Value = item.Lot;
                    reportSheet.Cells[salesRowStart, 4].Value = item.ProductName;
                    reportSheet.Cells[salesRowStart, 5].Value = item.SalePrice;
                    reportSheet.Cells[salesRowStart, 6].Value = item.PurchasePrice;
                    reportSheet.Cells[salesRowStart, 7].Value = item.Quantity;
                    reportSheet.Cells[salesRowStart, 8].Formula = $"E{salesRowStart}*G{salesRowStart}"; // Giá bán * Số lượng
                    salesTotal += item.SalePrice * item.Quantity;
                    salesRowStart++;
                }
                reportSheet.Cells[salesRowStart, 7].Value = "Thành tiền";
                reportSheet.Cells[salesRowStart, 8].Value = salesTotal;

                // Insert data for purchases
                decimal purchaseTotal = 0;
                int purchaseRowStart = 5; // Adjust based on actual content
                int columnOffset = 13;
                foreach (var item in purchaseData)
                {
                    reportSheet.Cells[purchaseRowStart, columnOffset + 1].Value = item.Index;
                    reportSheet.Cells[purchaseRowStart, columnOffset + 2].Value = item.Date.ToShortDateString();
                    reportSheet.Cells[purchaseRowStart, columnOffset + 3].Value = item.Lot;
                    reportSheet.Cells[purchaseRowStart, columnOffset + 4].Value = item.ProductName;
                    reportSheet.Cells[purchaseRowStart, columnOffset + 5].Value = item.PurchasePrice;
                    reportSheet.Cells[purchaseRowStart, columnOffset + 6].Value = item.Quantity;
                    reportSheet.Cells[purchaseRowStart, columnOffset + 7].Formula = $"R{purchaseRowStart}*S{purchaseRowStart}"; // Giá mua * Số lượng
                    purchaseTotal += item.PurchasePrice * item.Quantity;
                    purchaseRowStart++;
                }

                reportSheet.Cells[purchaseRowStart, columnOffset + 6].Value = "Thành tiền";
                reportSheet.Cells[purchaseRowStart, columnOffset + 7].Value = purchaseTotal;



                profitSheet.Cells["C5"].Formula = "'Báo cáo'!H" + salesRowStart.ToString(); // Link total sales to profit sheet
                profitSheet.Cells["C6"].Formula = "'Báo cáo'!T" + purchaseRowStart.ToString(); // Link total purchases to profit sheet
                profitSheet.Cells["C7"].Formula = "C5-C6"; // Calculate profit

                // Save to a new file to prevent modifying the template directly
                var newFile = new FileInfo("Updated_Report.xlsx");
                if (newFile.Exists)
                {
                    newFile.Delete();  // Ensures a new workbook is created
                }
                package.SaveAs(newFile);

                var stream = new MemoryStream();
                using (var fileStream = new FileStream(newFile.FullName, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                }
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Updated_Report.xlsx");
            }
        }

        [HttpGet]
        [Route("api/reports/bill/{invoiceId}")]
        public ActionResult GetBillReport(int invoiceId)
        {
            var report = new Report2(invoiceId, _unitOfWork);
            var stream = new MemoryStream();
            report.ExportToPdf(stream);
            stream.Position = 0;
            return File(stream, "application/pdf", "BillReport.pdf");
        }


    }
    public class Student_Report
    {
        public string MaHS { get; set; }
        public string HoHS { get; set; }
        public string TenHS { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string TenLOP { get; set; }
        public double Diem { get; set; }
        public string TenMH { get; set; }
    }
}
