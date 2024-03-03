using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
  [Table("user_role")]
  public class UserRole
  {
    public int Id { get; set; } = 0;
    public int RoleId { get; set; } = 0;
    [ForeignKey("RoleId")]
    [ValidateNever]
    public Role? Role { get; set; }

    public int UserId { get; set; } = 0;
    [ForeignKey("UserId")]
    [ValidateNever]
    public User? User { get; set; }
  }
}
