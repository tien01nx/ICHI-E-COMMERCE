using System.ComponentModel.DataAnnotations;

namespace ICHI_CORE.Domain
{
    public class User
    {
        [Key]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public int FailedPassAttemptCount { get; set; } = 0;

        [Required]
        public DateTime CreateDate { get; set; } = System.DateTime.Now;

        [Required]
        [StringLength(100)]
        public string CreateBy { get; set; } = "Admin";

        public DateTime? ModifiedDate { get; set; } = System.DateTime.Now;

        [StringLength(100)]
        public string? ModifiedBy { get; set; } = "Admin";
    }
}
