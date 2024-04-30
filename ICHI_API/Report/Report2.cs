using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_CORE.Domain.MasterModel;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace ICHI_API.Report
{
    public partial class Report2 : DevExpress.XtraReports.UI.XtraReport
    {

        private readonly IUnitOfWork _unitOfWork;
        public Report2(int invoiceId, IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            LoadData(invoiceId);
        }
        private void LoadData(int invoiceId)
        {
            var invoice = _unitOfWork.TrxTransaction.Get(i => i.Id == invoiceId, "Customer");
            //var invoiceDetail = _unitOfWork.TransactionDetail.Get(u => u.TrxTransactionId == invoiceId, "Product");
            var invoiceDetails = new BindingList<TransactionDetail>(_unitOfWork.TransactionDetail.GetAll(filter: u => u.TrxTransactionId == invoiceId, includeProperties: "Product").ToList());


            var total = invoiceDetails.Sum(detail => detail.Price * detail.Quantity);
            if (invoice != null)
            {
                // Điền dữ liệu vào các đối tượng tương ứng trong báo cáo
                Id.Text = invoice.Id.ToString();
                orderDate.Text = invoice.OrderDate.ToString("dd/mm/yyyy");
                Address.Text = "Thị trấn Ninh Giang";
                phone.Text = "0987654321";

                customerName.Text = "Tên khách hàng: " + invoice.Customer.FullName;
                customerAddress.Text = "Địa chỉ: " + invoice.Customer.Address;
                customerPhone.Text = "Số điện thoại: " + invoice.Customer.PhoneNumber;

                this.DataSource = invoiceDetails;

                ProductName.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Product.ProductName]"));
                Quantity.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Quantity]"));
                Price.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString('{0:n0}', [Price])"));
                money.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString('{0:n0}',[Price] * [Quantity])"));
                money.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString('{0:n0}',[Price] * [Quantity])"));
                totalmoney.Text = total.ToString("n0");
            }
        }
    }
}
