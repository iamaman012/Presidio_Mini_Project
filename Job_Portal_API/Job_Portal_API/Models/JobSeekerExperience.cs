namespace Job_Portal_API.Models
{
    public class JobSeekerExperience
    {
        public int ExperienceID { get; set; }
        public int JobSeekerID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        // Navigation property
        public JobSeeker JobSeeker { get; set; }
    }
}
