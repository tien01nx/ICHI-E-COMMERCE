using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class UserRegister
    {
        [Column("use_pwd")]
        [StringLength(64)]
        [Required]
        public string UsePassword { get; set; } = string.Empty;

        [Column("user_name")]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Column("full_name")]
        [StringLength(50)]
        public string FullName { get; set; } = string.Empty;

        [Column("phone_number")]
        [StringLength(20)]
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;


    }
}
