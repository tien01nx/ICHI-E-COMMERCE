using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCS_CORE.Domain
{
    public class MasterEntity
    {
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("create_datetime")]
        [Required]
        public DateTime CreateDatetime { get; set; } = System.DateTime.Now;

        [Column("create_user_id")]
        [Required]
        [StringLength(10)]
        public string CreateUserId { get; set; }

        [Column("update_datetime")]
        [Required]
        public DateTime UpdateDatetime { get; set; } = System.DateTime.Now;

        [Column("update_user_id")]
        [Required]
        [StringLength(10)]
        public string UpdateUserId { get; set; } 
    }
}
