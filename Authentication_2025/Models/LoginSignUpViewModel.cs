using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Authentication_2025.Models
{
    public class LoginSignUpViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; }

        public bool IsActive { get; set; }
    }
}
