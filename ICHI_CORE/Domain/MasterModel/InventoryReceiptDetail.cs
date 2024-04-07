namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InventoryReceiptDetail : MasterEntity
    {
        public int InventoryReceiptId { get; set; } = 0;

        [ForeignKey("InventoryReceiptId")]
        public InventoryReceipt? InventoryReceipt { get; set; }

        [Required(ErrorMessage = "ProductId là bắt buộc")]
        public int ProductId { get; set; } = 0;

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Tổng số phải lớn hơn hoặc bằng 0")]
        public double Total { get; set; } = 0;

        private double _version = 1;

        public double BatchNumber
        {
            get => _version;
            set => _version = IncrementVersion(value);
        }

        public double IncrementVersion(double version)
        {
            if (version <= 1.19)
            {
                // Tăng phiên bản lên 0.01
                return version + 0.01;
            }
            else
            {
                return Math.Ceiling(version) + 0.0;
            }
        }

    }
}
