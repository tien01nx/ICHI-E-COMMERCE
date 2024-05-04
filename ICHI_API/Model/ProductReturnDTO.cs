using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_API.Model
{
  public class ProductReturnDTO
  {
    public int Id { get; set; }
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

    public string? Reason { get; set; }

    public DateTime? CreateDate { get; set; }

    [Required(ErrorMessage = "Người tạo là bắt buộc")]
    [StringLength(100, ErrorMessage = "Người tạo không được vượt quá 100 ký tự")]
    public string CreateBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [StringLength(100, ErrorMessage = "Người sửa không được vượt quá 100 ký tự")]
    public string ModifiedBy { get; set; }

    public ProductReturn? _productReturn;


    [ValidateNever]
    public ProductReturn? ProductReturn
    {
      get { return _productReturn; }
      set
      {
        _productReturn = value;
        Id = _productReturn.Id;
        EmployeeId = _productReturn.EmployeeId;
        Employee = _productReturn.Employee;
        TrxTransactionId = _productReturn.TrxTransactionId;
        TrxTransaction = _productReturn.TrxTransaction;
        Status = _productReturn.Status;
        Reason = _productReturn.Reason;
        CreateDate = _productReturn.CreateDate;
        CreateBy = _productReturn.CreateBy;
        ModifiedDate = _productReturn.ModifiedDate;
        ModifiedBy = _productReturn.ModifiedBy;
      }
    }
    public IEnumerable<ProductReturnDetail> ReturnProductDetails { get; set; }

    public ProductReturnDTO()
    {
      ProductReturn = new ProductReturn();
      ReturnProductDetails = new List<ProductReturnDetail>();
    }
  }
}
