using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
  public class UserDTO
  {
    public User? User { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public bool? IsLocked { get; set; }
    public int? FailedPassAttemptCount { get; set; } = 0;
    public DateTime ModifiedDate { get; set; }
    public int Id { get; set; }
  }
}
