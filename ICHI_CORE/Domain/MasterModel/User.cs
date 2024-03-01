using ICHI_CORE.Domain.MasterModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain
{
  [Table("user")]
  public class User
  {
    [Key]
    [Column("user_id")]
    [Required]
    public string UserId { get; set; }

    [Column("user_name")]
    [Required]
    [StringLength(50)]
    public string UserName { get; set; }

    [Column("use_pwd")]
    [StringLength(64)]
    [Required]
    public string UsePassword { get; set; }


    [Column("full_name")]
    [StringLength(50)]
    public string FullName { get; set; }


    [Column("phone_number")]
    [StringLength(20)]
    [Required]
    public string PhoneNumber { get; set; }


    [Column("dateofbirth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Column("address")]
    [StringLength(100)]
    public string Address { get; set; } = string.Empty;

    [Column("avatar")]
    [StringLength(200)]
    public string avatar { get; set; } = string.Empty;

    [Column("is_active")]
    [Required]
    public bool Active { get; set; } = true;

    [Column("facebook_account_id")]
    public int facebookAccountId { get; set; } = 0;

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

    public ICollection<UserRole> UserRoles { get; set; }
  }
}
