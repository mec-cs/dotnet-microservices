using System.ComponentModel.DataAnnotations;
using Core.APP.Domain;

namespace Users.APP.Domain
{
    public class Role : Entity
    {
        [Required, StringLength(25)]
        public string Name { get; set; }
        
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
