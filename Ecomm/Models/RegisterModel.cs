using System.ComponentModel.DataAnnotations;

namespace Ecomm.Models
{
    public class RegisterModel // For making Register Method in Authenticate Controller for User Registering
    {
        [Required(ErrorMessage = "UserName is Required")] //Required Validation on props
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
