using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class InventoryReceiptDTO
  {
    public string EmployeeId { get; set; }

    public string? FullName { get; set; }

    public int SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? Notes { get; set; } = string.Empty;

    public List<InventoryReceiptDetail> InventoryReceiptDetails { get; set; } = new List<InventoryReceiptDetail>();
  }
}
