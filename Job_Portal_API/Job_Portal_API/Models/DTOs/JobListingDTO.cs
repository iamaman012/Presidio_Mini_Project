using Job_Portal_API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class JobListingDTO
    {
        [Required(ErrorMessage = "Job Title is required")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Job Description is required")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Job Type is required")]
        public string JobType { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public double Salary { get; set; }

        [Required(ErrorMessage = "Posting Date is required")]
        public DateTime PostingDate { get; set; }

        [Required(ErrorMessage = "Closing Date is required")]
        public DateTime ClosingDate { get; set; }

        [Required(ErrorMessage = "Employer ID is required")]
        public int EmployerID { get; set; }

        [Required(ErrorMessage = "Job Category is required")]
        public List<JobSkillDTO> Skills { get; set; }
    }
}
