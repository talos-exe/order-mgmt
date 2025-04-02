using System.ComponentModel.DataAnnotations;
using System.Data;

namespace OrderMgmtRevision.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }


        public DateTime? LastLoginDate { get; set; }
        public string? LastLoginIP { get; set; }
        //[Display(Name = "Role")]
        //public string Roles { get; set; }

        public List<UserLog>? Logs {  get; set; } = new List<UserLog>();
        public User? User { get; set; }
    }
}
