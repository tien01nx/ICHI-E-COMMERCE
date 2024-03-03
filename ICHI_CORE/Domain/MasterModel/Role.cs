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
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
