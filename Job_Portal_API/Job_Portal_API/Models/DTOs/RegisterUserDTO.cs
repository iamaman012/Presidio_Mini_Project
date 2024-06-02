using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class RegisterUserDTO
    {

        [Required (ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact number must be 10 digits long")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public string UserType { get; set; }
    }
}
