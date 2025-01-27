
using OrderManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class UsersRole
    {
        [StringLength(25)]
        public string UserId { get; set; }

        [StringLength(25)]
        public string RoleId { get; set; }

        // Navigation properties
        public User Users { get; set; }
        public Role Role { get; set; }
    }
}
