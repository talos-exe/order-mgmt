using System.ComponentModel.DataAnnotations;

namespace OrderMgmtRevision.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }
    }
}
