using ICHI_CORE.Domain.MasterModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain
{
  public class User : MasterEntity
  {
    public int Id { get; set; }
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    public bool IsLocked { get; set; } = false;
    public int FailedPassAttemptCount { get; set; } = 0;
  }
}
