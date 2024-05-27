using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class RegisterUserDTO
    {

        [Required (ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "User type is required")]
        public string UserType { get; set; }
    }
}
