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
    public class UserRole
    {
        public int Id { get; set; } = 0;
        public int RoleId { get; set; } = 0;
        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role? Role { get; set; }

        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }
    }
}
