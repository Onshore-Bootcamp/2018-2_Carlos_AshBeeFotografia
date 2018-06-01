using System.ComponentModel.DataAnnotations;

namespace AshBeeFotografia.Models
{
    public class UserPO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Password { get; set; }
    }
}