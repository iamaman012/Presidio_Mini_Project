namespace Job_Portal_API.Models
{
    public class JobSeekerEducation
    {
        public int EducationID { get; set; }
        public int JobSeekerID { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        // Navigation property
        public JobSeeker JobSeeker { get; set; }
    }
}
