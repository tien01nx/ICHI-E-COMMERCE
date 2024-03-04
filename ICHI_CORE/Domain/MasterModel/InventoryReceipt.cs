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
    public int SupplierID { get; set; }
    [ForeignKey("SupplierID")]
    [ValidateNever]
    public Supplier? Supplier { get; set; }
    public bool isAvtive { get; set; } = false;
  }
}
