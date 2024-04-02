namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;

  public class Supplier : MasterEntity
  {
    [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên nhà cung cấp phải có tối đa 255 ký tự")]
    public string SupplierName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mã số thuế là bắt buộc")]
    [StringLength(50, ErrorMessage = "Mã số thuế phải có tối đa 50 ký tự")]
    public string TaxCode { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Địa chỉ phải có tối đa 255 ký tự")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [StringLength(12, ErrorMessage = "Số điện thoại phải có tối đa 12 ký tự")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Email phải có tối đa 255 ký tự")]
    public string Email { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Số tài khoản ngân hàng phải có tối đa 50 ký tự")]
    public string BankAccount { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Tên ngân hàng phải có tối đa 100 ký tự")]
    public string BankName { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;
  }
}
