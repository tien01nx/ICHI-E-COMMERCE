using DevExpress.XtraReports.UI;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace ICHI_API.Report
{
    public partial class Bill : DevExpress.XtraReports.UI.XtraReport
    {
        private readonly IUnitOfWork _unitOfWork;
        public Bill(int invoiceId, IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            LoadData(invoiceId);
        }
        private void LoadData(int invoiceId)
        {
            var invoice = _unitOfWork.TrxTransaction.Get(i => i.Id == invoiceId);


            if (invoice != null)
            {
                // Điền dữ liệu vào các đối tượng tương ứng trong báo cáo
                invoiceNumber.Text = invoice.Id.ToString();
                //Parameters["invoiceDate"].Value = invoice.OrderDate;
                //Parameters["customerName"].Value = invoice.Customer.FullName;
                //Parameters["vendorContactName"].Value = invoice.Customer.FullName;
            }
        }
    }
}
