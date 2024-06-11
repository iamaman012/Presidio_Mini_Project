using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class AddEmployerDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Company Description is required")]
        public string CompanyDescription { get; set; }

        [Required(ErrorMessage = "Company Location is required")]
        public string CompanyLocation { get; set; }
    }
}
