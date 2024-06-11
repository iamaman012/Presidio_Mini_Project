using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class ExperienceDTO
    {
        [Required(ErrorMessage ="JobSeekerID is required")]
        
        public int JobSeekerID { get; set; }

        [Required(ErrorMessage ="Job Title is Required")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage ="Company Name Required")]
        public string CompanyName { get; set; }

        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
