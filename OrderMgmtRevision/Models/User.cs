using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class User
    {
        public int UserID { get; set; } // User ID for SQL

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } // User's first name

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } // users last name

        [Required]
        [MaxLength(50)]
        public string Username {  get; set; } // username for user

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } // user's email

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } // hashed password

        public string Role { get; set; } = "User"; // user's role can be administrator or user, 'Admin' or 'User'

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
