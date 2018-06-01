namespace AshBeeFotografia.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginPO
    {
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Password { get; set; }
    }
}