namespace Job_Portal_API.Models
{
    public class Application
    {
        public int ApplicationID { get; set; }
        public int JobID { get; set; }
        public int JobSeekerID { get; set; }
        public DateTime ApplicationDate { get; set; }=DateTime.Now;
        public string Status { get; set; }

        // Navigation properties
        public JobListing JobListing { get; set; }
        public JobSeeker JobSeeker { get; set; }
    }
}
