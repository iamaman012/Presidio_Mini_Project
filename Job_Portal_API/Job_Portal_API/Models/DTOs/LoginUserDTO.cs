using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage="Email is Required")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
