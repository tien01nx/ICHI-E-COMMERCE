using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  [Table("supplier")]
  public class Supplier : MasterEntity
  {
    [Required]
    [StringLength(255)]
    public string SupplierName { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string TaxCode { get; set; } = string.Empty;
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;
    [Required]
    [StringLength(12)]
    public string PhoneNumber { get; set; } = string.Empty;
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    [StringLength(50)]
    public string BankAccount { get; set; } = string.Empty;
    [StringLength(100)]
    public string BankName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool isActive { get; set; } = false;
    public bool isDeleted { get; set; } = false;

  }
}
