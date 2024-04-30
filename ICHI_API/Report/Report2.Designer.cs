namespace ICHI_API.Report
{
    partial class Report2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report2));
            DevExpress.DataAccess.EntityFramework.EFConnectionParameters efConnectionParameters1 = new DevExpress.DataAccess.EntityFramework.EFConnectionParameters();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.detailTable = new DevExpress.XtraReports.UI.XRTable();
            this.detailTableRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.ProductName = new DevExpress.XtraReports.UI.XRTableCell();
            this.Quantity = new DevExpress.XtraReports.UI.XRTableCell();
            this.Price = new DevExpress.XtraReports.UI.XRTableCell();
            this.money = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.invoiceInfoTable = new DevExpress.XtraReports.UI.XRTable();
            this.invoiceInfoTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.invoiceLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceNumberRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.invoiceNumberCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.Id = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceDateRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.invoiceDateCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.orderDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.customerTable = new DevExpress.XtraReports.UI.XRTable();
            this.customerTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.billToLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.customerNameRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.customerName = new DevExpress.XtraReports.UI.XRTableCell();
            this.customerAddressRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.customerAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.customerPhoneRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.customerPhone = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorTable = new DevExpress.XtraReports.UI.XRTable();
            this.vendorNameRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.vendorName = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorAddressRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.Address = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorPhoneRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.phone = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.totalTable = new DevExpress.XtraReports.UI.XRTable();
            this.totalRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.totalCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.totalmoney = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.headerTable = new DevExpress.XtraReports.UI.XRTable();
            this.headerTableRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.productDescriptionCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.quantityCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.unitPriceCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.lineTotalCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.efDataSource1 = new DevExpress.DataAccess.EntityFramework.EFDataSource(this.components);
            this.baseControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.detailTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceInfoTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.detailTable});
            this.Detail.HeightF = 40.00001F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "baseControlStyle";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // detailTable
            // 
            this.detailTable.BackColor = System.Drawing.Color.WhiteSmoke;
            this.detailTable.LocationFloat = new DevExpress.Utils.PointFloat(10.00008F, 10.00001F);
            this.detailTable.Name = "detailTable";
            this.detailTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.detailTableRow});
            this.detailTable.SizeF = new System.Drawing.SizeF(709.9991F, 30F);
            this.detailTable.StylePriority.UseBackColor = false;
            this.detailTable.StylePriority.UseFont = false;
            // 
            // detailTableRow
            // 
            this.detailTableRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.ProductName,
            this.Quantity,
            this.Price,
            this.money});
            this.detailTableRow.Name = "detailTableRow";
            this.detailTableRow.Weight = 10.58D;
            // 
            // ProductName
            // 
            this.ProductName.Name = "ProductName";
            this.ProductName.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 5, 5, 5, 100F);
            this.ProductName.StylePriority.UsePadding = false;
            this.ProductName.Text = "ProductName";
            this.ProductName.Weight = 1.680046589873347D;
            // 
            // Quantity
            // 
            this.Quantity.Name = "Quantity";
            this.Quantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 5, 100F);
            this.Quantity.StylePriority.UsePadding = false;
            this.Quantity.Text = "ProductDescription";
            this.Quantity.Weight = 0.33203838312934841D;
            // 
            // Price
            // 
            this.Price.Name = "Price";
            this.Price.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 5, 100F);
            this.Price.StylePriority.UsePadding = false;
            this.Price.StylePriority.UseTextAlignment = false;
            this.Price.Text = "1";
            this.Price.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Price.Weight = 0.37259230076829319D;
            // 
            // money
            // 
            this.money.Name = "money";
            this.money.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 5, 100F);
            this.money.StylePriority.UsePadding = false;
            this.money.StylePriority.UseTextAlignment = false;
            this.money.Text = "1.000";
            this.money.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.money.TextFormatString = "{0:n0}";
            this.money.Weight = 0.546505217293736D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 65F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseBackColor = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.invoiceInfoTable,
            this.customerTable,
            this.vendorTable,
            this.vendorLogo});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("InvoiceNumber", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.GroupHeader2.HeightF = 218F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.StyleName = "baseControlStyle";
            this.GroupHeader2.StylePriority.UseBackColor = false;
            // 
            // invoiceInfoTable
            // 
            this.invoiceInfoTable.LocationFloat = new DevExpress.Utils.PointFloat(375F, 10.00001F);
            this.invoiceInfoTable.Name = "invoiceInfoTable";
            this.invoiceInfoTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.invoiceInfoTableRow1,
            this.invoiceNumberRow,
            this.invoiceDateRow});
            this.invoiceInfoTable.SizeF = new System.Drawing.SizeF(344.9992F, 90F);
            // 
            // invoiceInfoTableRow1
            // 
            this.invoiceInfoTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.invoiceLabel});
            this.invoiceInfoTableRow1.Name = "invoiceInfoTableRow1";
            this.invoiceInfoTableRow1.StylePriority.UseFont = false;
            this.invoiceInfoTableRow1.StylePriority.UseTextAlignment = false;
            this.invoiceInfoTableRow1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.invoiceInfoTableRow1.Weight = 2.0000001203963937D;
            // 
            // invoiceLabel
            // 
            this.invoiceLabel.Font = new DevExpress.Drawing.DXFont("Segoe UI", 20.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.invoiceLabel.Name = "invoiceLabel";
            this.invoiceLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 0, 0, 100F);
            this.invoiceLabel.StylePriority.UseFont = false;
            this.invoiceLabel.StylePriority.UsePadding = false;
            this.invoiceLabel.StylePriority.UseTextAlignment = false;
            this.invoiceLabel.Text = "Hóa Đơn Bán Hàng";
            this.invoiceLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.invoiceLabel.Weight = 1.6426447605650873D;
            // 
            // invoiceNumberRow
            // 
            this.invoiceNumberRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.invoiceNumberCaption,
            this.Id});
            this.invoiceNumberRow.Name = "invoiceNumberRow";
            this.invoiceNumberRow.Weight = 0.80000006941199275D;
            // 
            // invoiceNumberCaption
            // 
            this.invoiceNumberCaption.CanShrink = true;
            this.invoiceNumberCaption.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.invoiceNumberCaption.Name = "invoiceNumberCaption";
            this.invoiceNumberCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 0, 0, 100F);
            this.invoiceNumberCaption.StylePriority.UseFont = false;
            this.invoiceNumberCaption.StylePriority.UsePadding = false;
            this.invoiceNumberCaption.StylePriority.UseTextAlignment = false;
            this.invoiceNumberCaption.Text = "Mã hóa đơn #";
            this.invoiceNumberCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.invoiceNumberCaption.Weight = 0.54753422741896873D;
            // 
            // Id
            // 
            this.Id.CanShrink = true;
            this.Id.Name = "Id";
            this.Id.StylePriority.UseFont = false;
            this.Id.StylePriority.UsePadding = false;
            this.Id.Text = "000001";
            this.Id.Weight = 1.0951105331461186D;
            // 
            // invoiceDateRow
            // 
            this.invoiceDateRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.invoiceDateCaption,
            this.orderDate});
            this.invoiceDateRow.Name = "invoiceDateRow";
            this.invoiceDateRow.Weight = 0.80000006941199264D;
            // 
            // invoiceDateCaption
            // 
            this.invoiceDateCaption.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.invoiceDateCaption.Name = "invoiceDateCaption";
            this.invoiceDateCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 0, 0, 100F);
            this.invoiceDateCaption.StylePriority.UseFont = false;
            this.invoiceDateCaption.StylePriority.UsePadding = false;
            this.invoiceDateCaption.StylePriority.UseTextAlignment = false;
            this.invoiceDateCaption.Text = "Ngày đặt";
            this.invoiceDateCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.invoiceDateCaption.Weight = 0.54753422741896873D;
            // 
            // orderDate
            // 
            this.orderDate.Name = "orderDate";
            this.orderDate.StylePriority.UseFont = false;
            this.orderDate.StylePriority.UsePadding = false;
            this.orderDate.Text = "InvoiceDate";
            this.orderDate.TextFormatString = "{0:d MMMM yyyy}";
            this.orderDate.Weight = 1.0951105331461186D;
            // 
            // customerTable
            // 
            this.customerTable.LocationFloat = new DevExpress.Utils.PointFloat(109.9993F, 130F);
            this.customerTable.Name = "customerTable";
            this.customerTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.customerTableRow1,
            this.customerNameRow,
            this.customerAddressRow,
            this.customerPhoneRow});
            this.customerTable.SizeF = new System.Drawing.SizeF(250F, 65F);
            // 
            // customerTableRow1
            // 
            this.customerTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.billToLabel});
            this.customerTableRow1.Name = "customerTableRow1";
            this.customerTableRow1.Weight = 1.0000001017252733D;
            // 
            // billToLabel
            // 
            this.billToLabel.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.billToLabel.Name = "billToLabel";
            this.billToLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 0, 0, 100F);
            this.billToLabel.StylePriority.UseFont = false;
            this.billToLabel.StylePriority.UsePadding = false;
            this.billToLabel.StylePriority.UseTextAlignment = false;
            this.billToLabel.Text = "Khách Hàng";
            this.billToLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.billToLabel.Weight = 2.3302581457621914D;
            // 
            // customerNameRow
            // 
            this.customerNameRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.customerName});
            this.customerNameRow.Name = "customerNameRow";
            this.customerNameRow.Weight = 0.80000011444093255D;
            // 
            // customerName
            // 
            this.customerName.CanShrink = true;
            this.customerName.Name = "customerName";
            this.customerName.StylePriority.UseFont = false;
            this.customerName.StylePriority.UsePadding = false;
            this.customerName.Text = "Tên khách hàng";
            this.customerName.Weight = 2.3302581457621914D;
            // 
            // customerAddressRow
            // 
            this.customerAddressRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.customerAddress});
            this.customerAddressRow.Name = "customerAddressRow";
            this.customerAddressRow.Weight = 0.80000011444093244D;
            // 
            // customerAddress
            // 
            this.customerAddress.CanShrink = true;
            this.customerAddress.Name = "customerAddress";
            this.customerAddress.Text = "Địa chỉ";
            this.customerAddress.Weight = 2.3302581457621914D;
            // 
            // customerPhoneRow
            // 
            this.customerPhoneRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.customerPhone});
            this.customerPhoneRow.Name = "customerPhoneRow";
            this.customerPhoneRow.Weight = 0.80000011444093244D;
            // 
            // customerPhone
            // 
            this.customerPhone.CanShrink = true;
            this.customerPhone.Name = "customerPhone";
            this.customerPhone.Text = "Số điện thoại";
            this.customerPhone.Weight = 2.3302581457621914D;
            // 
            // vendorTable
            // 
            this.vendorTable.LocationFloat = new DevExpress.Utils.PointFloat(110F, 10.00001F);
            this.vendorTable.Name = "vendorTable";
            this.vendorTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.vendorNameRow,
            this.vendorAddressRow,
            this.vendorPhoneRow});
            this.vendorTable.SizeF = new System.Drawing.SizeF(250F, 90F);
            // 
            // vendorNameRow
            // 
            this.vendorNameRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.vendorName});
            this.vendorNameRow.Name = "vendorNameRow";
            this.vendorNameRow.StylePriority.UseTextAlignment = false;
            this.vendorNameRow.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.vendorNameRow.Weight = 1.8181822191978998D;
            // 
            // vendorName
            // 
            this.vendorName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.vendorName.Name = "vendorName";
            this.vendorName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.vendorName.StylePriority.UseFont = false;
            this.vendorName.StylePriority.UsePadding = false;
            this.vendorName.Text = "Văn Phòng Phẩm ICHI";
            this.vendorName.Weight = 1D;
            // 
            // vendorAddressRow
            // 
            this.vendorAddressRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.Address});
            this.vendorAddressRow.Name = "vendorAddressRow";
            this.vendorAddressRow.Weight = 0.72727266295882753D;
            // 
            // Address
            // 
            this.Address.CanShrink = true;
            this.Address.Name = "Address";
            this.Address.StylePriority.UseFont = false;
            this.Address.StylePriority.UsePadding = false;
            this.Address.Text = "VendorAddress";
            this.Address.Weight = 1D;
            // 
            // vendorPhoneRow
            // 
            this.vendorPhoneRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.phone});
            this.vendorPhoneRow.Name = "vendorPhoneRow";
            this.vendorPhoneRow.Weight = 0.72727273231695466D;
            // 
            // phone
            // 
            this.phone.CanShrink = true;
            this.phone.Name = "phone";
            this.phone.StylePriority.UseFont = false;
            this.phone.Text = "VendorPhone";
            this.phone.Weight = 1D;
            // 
            // vendorLogo
            // 
            this.vendorLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleLeft;
            this.vendorLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("vendorLogo.ImageSource"));
            this.vendorLogo.LocationFloat = new DevExpress.Utils.PointFloat(9.999164F, 10.00001F);
            this.vendorLogo.Name = "vendorLogo";
            this.vendorLogo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.vendorLogo.SizeF = new System.Drawing.SizeF(85.00174F, 50F);
            this.vendorLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.vendorLogo.StylePriority.UseBorderColor = false;
            this.vendorLogo.StylePriority.UseBorders = false;
            this.vendorLogo.StylePriority.UsePadding = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.totalTable});
            this.GroupFooter1.GroupUnion = DevExpress.XtraReports.UI.GroupFooterUnion.WithLastDetail;
            this.GroupFooter1.HeightF = 58F;
            this.GroupFooter1.KeepTogether = true;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBandExceptLastEntry;
            this.GroupFooter1.StyleName = "baseControlStyle";
            // 
            // totalTable
            // 
            this.totalTable.BorderColor = System.Drawing.Color.White;
            this.totalTable.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.totalTable.BorderWidth = 10F;
            this.totalTable.ForeColor = System.Drawing.Color.Black;
            this.totalTable.LocationFloat = new DevExpress.Utils.PointFloat(374.9999F, 22.50001F);
            this.totalTable.Name = "totalTable";
            this.totalTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.totalRow});
            this.totalTable.SizeF = new System.Drawing.SizeF(345.0002F, 35F);
            this.totalTable.StylePriority.UseBorderColor = false;
            this.totalTable.StylePriority.UseBorders = false;
            this.totalTable.StylePriority.UseBorderWidth = false;
            this.totalTable.StylePriority.UseForeColor = false;
            // 
            // totalRow
            // 
            this.totalRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.totalCaption,
            this.totalmoney});
            this.totalRow.Name = "totalRow";
            this.totalRow.Weight = 1.3999999999999997D;
            // 
            // totalCaption
            // 
            this.totalCaption.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.totalCaption.Name = "totalCaption";
            this.totalCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 5, 100F);
            this.totalCaption.StylePriority.UseFont = false;
            this.totalCaption.StylePriority.UsePadding = false;
            this.totalCaption.StylePriority.UseTextAlignment = false;
            this.totalCaption.Text = "Tổng tiền";
            this.totalCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.totalCaption.Weight = 0.84464081370499255D;
            // 
            // totalmoney
            // 
            this.totalmoney.BackColor = System.Drawing.Color.WhiteSmoke;
            this.totalmoney.Name = "totalmoney";
            this.totalmoney.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 0, 5, 100F);
            this.totalmoney.StylePriority.UseBackColor = false;
            this.totalmoney.StylePriority.UseFont = false;
            this.totalmoney.StylePriority.UsePadding = false;
            this.totalmoney.StylePriority.UseTextAlignment = false;
            this.totalmoney.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.totalmoney.TextFormatString = "{0:n0}";
            this.totalmoney.Weight = 1.6893521735999855D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.headerTable});
            this.GroupHeader1.HeightF = 25F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.RepeatEveryPage = true;
            this.GroupHeader1.StyleName = "baseControlStyle";
            // 
            // headerTable
            // 
            this.headerTable.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.headerTable.LocationFloat = new DevExpress.Utils.PointFloat(10.00006F, 0F);
            this.headerTable.Name = "headerTable";
            this.headerTable.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
            this.headerTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.headerTableRow});
            this.headerTable.SizeF = new System.Drawing.SizeF(709.9999F, 25F);
            this.headerTable.StylePriority.UseFont = false;
            this.headerTable.StylePriority.UsePadding = false;
            // 
            // headerTableRow
            // 
            this.headerTableRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.productDescriptionCaption,
            this.quantityCaption,
            this.unitPriceCaption,
            this.lineTotalCaption});
            this.headerTableRow.Name = "headerTableRow";
            this.headerTableRow.Weight = 11.5D;
            // 
            // productDescriptionCaption
            // 
            this.productDescriptionCaption.Name = "productDescriptionCaption";
            this.productDescriptionCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.productDescriptionCaption.StylePriority.UsePadding = false;
            this.productDescriptionCaption.Text = "Tên Sản Phẩm";
            this.productDescriptionCaption.Weight = 2.1136148199486358D;
            // 
            // quantityCaption
            // 
            this.quantityCaption.Name = "quantityCaption";
            this.quantityCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.quantityCaption.StylePriority.UsePadding = false;
            this.quantityCaption.StylePriority.UseTextAlignment = false;
            this.quantityCaption.Text = "Số Lượng";
            this.quantityCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.quantityCaption.Weight = 0.41772702684745394D;
            // 
            // unitPriceCaption
            // 
            this.unitPriceCaption.Name = "unitPriceCaption";
            this.unitPriceCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.unitPriceCaption.StylePriority.UsePadding = false;
            this.unitPriceCaption.StylePriority.UseTextAlignment = false;
            this.unitPriceCaption.Text = "Đơn Giá";
            this.unitPriceCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.unitPriceCaption.Weight = 0.46874727898816382D;
            // 
            // lineTotalCaption
            // 
            this.lineTotalCaption.Name = "lineTotalCaption";
            this.lineTotalCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 10, 5, 0, 100F);
            this.lineTotalCaption.StylePriority.UsePadding = false;
            this.lineTotalCaption.StylePriority.UseTextAlignment = false;
            this.lineTotalCaption.Text = "Thành Tiền";
            this.lineTotalCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lineTotalCaption.Weight = 0.68754517279656213D;
            // 
            // efDataSource1
            // 
            efConnectionParameters1.ConnectionString = "";
            efConnectionParameters1.ConnectionStringName = "DefaultConnection";
            efConnectionParameters1.Source = typeof(global::ICHI_API.Data.PcsApiContext);
            this.efDataSource1.ConnectionParameters = efConnectionParameters1;
            this.efDataSource1.Name = "efDataSource1";
            // 
            // baseControlStyle
            // 
            this.baseControlStyle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9F);
            this.baseControlStyle.Name = "baseControlStyle";
            this.baseControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // Report2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupHeader1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.efDataSource1});
            this.DataSource = this.efDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(60F, 60F, 65F, 100F);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.baseControlStyle});
            this.Version = "22.2";
            ((System.ComponentModel.ISupportInitialize)(this.detailTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceInfoTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable detailTable;
        private DevExpress.XtraReports.UI.XRTableRow detailTableRow;
        private DevExpress.XtraReports.UI.XRTableCell ProductName;
        private DevExpress.XtraReports.UI.XRTableCell Quantity;
        private DevExpress.XtraReports.UI.XRTableCell Price;
        private DevExpress.XtraReports.UI.XRTableCell money;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRTable invoiceInfoTable;
        private DevExpress.XtraReports.UI.XRTableRow invoiceInfoTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell invoiceLabel;
        private DevExpress.XtraReports.UI.XRTableRow invoiceNumberRow;
        private DevExpress.XtraReports.UI.XRTableCell invoiceNumberCaption;
        private DevExpress.XtraReports.UI.XRTableCell Id;
        private DevExpress.XtraReports.UI.XRTableRow invoiceDateRow;
        private DevExpress.XtraReports.UI.XRTableCell invoiceDateCaption;
        private DevExpress.XtraReports.UI.XRTableCell orderDate;
        private DevExpress.XtraReports.UI.XRTable customerTable;
        private DevExpress.XtraReports.UI.XRTableRow customerTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell billToLabel;
        private DevExpress.XtraReports.UI.XRTableRow customerNameRow;
        private DevExpress.XtraReports.UI.XRTableCell customerName;
        private DevExpress.XtraReports.UI.XRTableRow customerAddressRow;
        private DevExpress.XtraReports.UI.XRTableRow customerPhoneRow;
        private DevExpress.XtraReports.UI.XRTableCell customerAddress;
        private DevExpress.XtraReports.UI.XRTableCell customerPhone;
        private DevExpress.XtraReports.UI.XRTable vendorTable;
        private DevExpress.XtraReports.UI.XRTableRow vendorNameRow;
        private DevExpress.XtraReports.UI.XRTableCell vendorName;
        private DevExpress.XtraReports.UI.XRTableRow vendorAddressRow;
        private DevExpress.XtraReports.UI.XRTableCell Address;
        private DevExpress.XtraReports.UI.XRTableRow vendorPhoneRow;
        private DevExpress.XtraReports.UI.XRTableCell phone;
        private DevExpress.XtraReports.UI.XRPictureBox vendorLogo;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable totalTable;
        private DevExpress.XtraReports.UI.XRTableRow totalRow;
        private DevExpress.XtraReports.UI.XRTableCell totalCaption;
        private DevExpress.XtraReports.UI.XRTableCell totalmoney;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable headerTable;
        private DevExpress.XtraReports.UI.XRTableRow headerTableRow;
        private DevExpress.XtraReports.UI.XRTableCell productDescriptionCaption;
        private DevExpress.XtraReports.UI.XRTableCell quantityCaption;
        private DevExpress.XtraReports.UI.XRTableCell unitPriceCaption;
        private DevExpress.XtraReports.UI.XRTableCell lineTotalCaption;
        private DevExpress.DataAccess.EntityFramework.EFDataSource efDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle baseControlStyle;
    }
}
