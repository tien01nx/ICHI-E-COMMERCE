using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Address : MasterEntity
  {
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public bool isDefault { get; set; } = false;

    public string CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    [ValidateNever]
    public Customer? Customer { get; set; }
  }
}
