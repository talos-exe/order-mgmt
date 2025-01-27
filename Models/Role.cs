using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class Role
    {
        [Key]
        [Column("Role_ID")]
        [StringLength(25)]
        public string RoleId { get; set; }

        [Required]
        [StringLength(25)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string RoleDescription { get; set; }

        // Navigation properties
        public ICollection<UsersRole> UserRole { get; set; } = new List<UsersRole>();
    }
}