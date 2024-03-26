using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Customer : MasterEntity
  {
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }

    [Required]
    [StringLength(255)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string Gender { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [StringLength(12)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public bool isActive { get; set; } = false;

    public bool isDeleted { get; set; } = false;
  }
}
