using Job_Portal_API.Models.Enums;

namespace Job_Portal_API.Models.DTOs
{
    public class JobListingDTO
    {
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobType { get; set; }
        public string Location { get; set; }
        public double Salary { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public int EmployerID { get; set; }
        public List<JobSkillDTO> Skills { get; set; }
    }
}
