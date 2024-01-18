using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
    [Table("log")]
    public class Log
    {
        [Column("log_time")]
        public DateTime? LogTime { get; set; }

        [Column("log_status")]
        public byte? LogStatus { get; set; }

        [Column("pc_name")]
        [StringLength(50)]
        public string? PcName { get; set; }

        [Column("pg_id")]
        [StringLength(50)]
        public string? PgId { get; set; }

        [Column("user_id")]
        [StringLength(10)]
        public string? UserId { get; set; }

        [Column("message")]
        public string? Message { get; set; }
    }
}
