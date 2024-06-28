namespace Job_Portal_API.Models.DTOs
{
    public class ApplicationResponseDTO
    {
        public int ApplicationID { get; set; }
        public int JobID { get; set; }
        public int JobSeekerID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public JobListingResponseDTO JobListing { get; set; }

    }
}
