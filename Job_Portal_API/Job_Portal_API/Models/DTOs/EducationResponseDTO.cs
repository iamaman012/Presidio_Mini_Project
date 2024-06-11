using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class EducationResponseDTO
    {
        [Required(ErrorMessage = "Education ID is required")]
        public int EducationID { get; set; }

        [Required(ErrorMessage = "JobSeeker ID is required")]
        public int JobSeekerID { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Institution is required")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "GPA is required")]
        public double GPA { get; set; } 
    }
}
