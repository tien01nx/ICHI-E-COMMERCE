namespace ICHI_CORE.Domain.MasterModel
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

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
