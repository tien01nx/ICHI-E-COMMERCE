using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.WebSockets;
using System.Text;

namespace ICHI_CORE.Controllers.MasterController
{
    [Route("api/ws")]
    public class WebSocketController : ControllerBase
    {
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
        [HttpGet("testhehe")]
        public IActionResult TestHehe()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Prepare the memory stream to hold the excel data
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                // Add a sheet for "Sales Invoice Details"
                var salesSheet = package.Workbook.Worksheets.Add("Sales Invoice Details");
                // Headers
                string[] salesHeaders = new string[] { "STT", "Số Lô", "Mã sản phẩm", "Tên sản phẩm", "Giá bán", "Giá nhập", "Số lượng", "Thành tiền" };
                for (int i = 0; i < salesHeaders.Length; i++)
                {
                    salesSheet.Cells[1, i + 1].Value = salesHeaders[i];
                }
                // Add a sample data row
                salesSheet.Cells["A2"].Value = 1;
                salesSheet.Cells["B2"].Value = "L001";
                salesSheet.Cells["C2"].Value = "P001";
                salesSheet.Cells["D2"].Value = "Product 1";
                salesSheet.Cells["E2"].Value = 100;
                salesSheet.Cells["F2"].Value = 80;
                salesSheet.Cells["G2"].Value = 10;
                salesSheet.Cells["H2"].Formula = "E2*G2";  // Thành tiền

                // Add a sheet for "Purchase Invoice Details"
                var purchaseSheet = package.Workbook.Worksheets.Add("Purchase Invoice Details");
                // Headers
                string[] purchaseHeaders = new string[] { "STT", "Số Lô", "Mã sản phẩm", "Tên sản phẩm", "Giá nhập", "Số lượng", "Thành tiền" };
                for (int j = 0; j < purchaseHeaders.Length; j++)
                {
                    purchaseSheet.Cells[1, j + 1].Value = purchaseHeaders[j];
                }
                // Add a sample data row
                purchaseSheet.Cells["A2"].Value = 1;
                purchaseSheet.Cells["B2"].Value = "L001";
                purchaseSheet.Cells["C2"].Value = "P001";
                purchaseSheet.Cells["D2"].Value = "Product 1";
                purchaseSheet.Cells["E2"].Value = 80;
                purchaseSheet.Cells["F2"].Value = 10;
                purchaseSheet.Cells["G2"].Formula = "E2*F2";  // Thành tiền

                // Save the changes and prepare the file for download
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Report-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            // Return the stream as a file attachment in the response
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
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
