using API.Model;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Model;
using ICHI_API.Report;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("bill/{invoiceId}")]
        public ActionResult GetBillReport(int invoiceId)
        {
            var report = new Report2(invoiceId, _unitOfWork);
            var stream = new MemoryStream();
            report.ExportToPdf(stream);
            stream.Position = 0;


            var contentDispositionHeader = new System.Net.Mime.ContentDisposition
            {
                FileName = "BillReport.pdf",
                Inline = true  // False = prompt the user for downloading; True = try to open in browser
            };
            Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff"); // Để tránh trình duyệt đoán MIME type

            return File(stream, "application/pdf");

            //return File(stream, "application/pdf", "BillReport.pdf");
        }

    }
}
