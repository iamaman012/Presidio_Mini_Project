namespace Job_Portal_API.Models.DTOs
{
    public class ApplicationResponseDTO
    {
        public int ApplicationID { get; set; }
        public int JobID { get; set; }
        public int JobSeekerID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public string? JobSeekerName { get; set; }   
        public string? JobTitle { get; set; }
        public string? JobType { get; set; }

        public double? Salary { get; set; }
        public string? Location { get; set; }

        public string? CompanyImage { get; set; }   

        public JobListingResponseDTO? JobListing { get; set; }

    }
}
