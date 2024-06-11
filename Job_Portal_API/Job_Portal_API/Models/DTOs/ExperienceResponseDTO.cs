using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class ExperienceResponseDTO
    {
        [Required(ErrorMessage = "Experience ID is required")]
        public int ExperienceID { get; set; }

        [Required(ErrorMessage = "JobSeeker ID is required")]
        public int JobSeekerID { get; set; }

        [Required(ErrorMessage = "Job Title is required")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
