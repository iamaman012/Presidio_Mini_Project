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

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; }

        
        public DateTime PostingDate { get; set; }

        [Required(ErrorMessage = "Closing Date is required")]
        public DateTime ClosingDate { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Company Description is required")]
        public string CompanyDescription { get; set; }
        [Required(ErrorMessage = "Company Location is required")]
        public string CompanyLocation { get; set; }

        [Required(ErrorMessage = "Employer ID is required")]
        public int EmployerID { get; set; }

        [Required(ErrorMessage = "Job Category is required")]
        public List<String> Skills { get; set; }
    }
}
