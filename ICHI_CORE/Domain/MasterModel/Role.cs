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
    [Table("role")]
    
    public class Role : MasterEntity
    {
        [Column("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
