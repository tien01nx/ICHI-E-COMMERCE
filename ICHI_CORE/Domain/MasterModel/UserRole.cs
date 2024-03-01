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
    [Key]
    public int Id { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    public Role Role { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    public User User { get; set; }

  }
}
