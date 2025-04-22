using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace OrderMgmtRevision.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string? LastLoginIP { get; set; }
        //[Display(Name = "Role")]
        //public string Roles { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsActive {  get; set; }

        public decimal? AccountBalance { get; set; } = 0;

        public bool IsAdmin {  get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;

        public List<UserLog>? Logs {  get; set; } = new List<UserLog>();
        public User? User { get; set; }
    }
}
