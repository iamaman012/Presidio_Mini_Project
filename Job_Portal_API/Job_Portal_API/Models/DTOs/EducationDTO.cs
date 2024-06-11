using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class EducationDTO
    {
        [Required(ErrorMessage ="JobSeekerID is Required")]
        public int JobSeekerID { get; set; }

        [Required]

        public string Degree { get; set; }

        [Required]
        public string Institution { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public double GPA { get; set; } 
    }
}
