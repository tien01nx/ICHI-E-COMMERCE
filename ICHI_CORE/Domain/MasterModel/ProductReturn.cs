namespace ICHI_CORE.Domain.MasterModel
{
  using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class ProductReturn : MasterEntity
  {
    [Required(ErrorMessage = "Nhân viên là bắt buộc")]
    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    [ValidateNever]
    public Employee? Employee { get; set; }

    [Required(ErrorMessage = "Mã hóa đơn là bắt buộc")]
    public int TrxTransactionId { get; set; }

    [ForeignKey("TrxTransactionId")]
    [ValidateNever]
    public TrxTransaction? TrxTransaction { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Trạng thái")]
    public string Status { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Lý do đổi trả")]
    public string Reason { get; set; }

  }
}
