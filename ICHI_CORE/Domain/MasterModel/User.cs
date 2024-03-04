using ICHI_CORE.Domain.MasterModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_CORE.Domain
{
  public class User : MasterEntity
  {
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    public bool IsLocked { get; set; } = false;
    public int FailedPassAttemptCount { get; set; } = 0;
  }
}
