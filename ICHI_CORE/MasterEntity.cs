using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain
{
  public class MasterEntity
  {
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime CreateDate { get; set; } = System.DateTime.Now;

    [Required]
    [StringLength(100)]
    public string CreateBy { get; set; } = "Admin";

    [Required]
    public DateTime ModifiedDate { get; set; } = System.DateTime.Now;

    [Required]
    [StringLength(100)]
    public string ModifiedBy { get; set; } = "Admin";
  }
}
