namespace Job_Portal_API.Models.DTOs
{
    public class ExperienceDTO
    {
        public int JobSeekerID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
