using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  public class InventoryReceipt : MasterEntity
  {
    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    [ValidateNever]
    public Employee? Employee { get; set; }

    public int SupplierId { get; set; }

    [ForeignKey("SupplierId")]
    [ValidateNever]
    public Supplier? Supplier { get; set; }

    public string? Notes { get; set; } = string.Empty;

    public bool isAvtive { get; set; } = false;
  }
}
