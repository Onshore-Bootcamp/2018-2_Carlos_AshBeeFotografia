
namespace DataLayer.Models
{
    public class UserDO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
