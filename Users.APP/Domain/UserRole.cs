using Core.APP.Domain;

namespace Users.APP.Domain
{
    public class UserRole : Entity
    {
        public int UserId { get; set; }
        
        public User User { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}