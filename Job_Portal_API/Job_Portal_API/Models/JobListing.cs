namespace Job_Portal_API.Models
{
    public class JobListing
    {
        
        
            public int JobID { get; set; }
            public string JobTitle { get; set; }
            public string JobDescription { get; set; }
            public string JobType { get; set; }
            public string Location { get; set; }
            public Double Salary { get; set; }
            public DateTime PostingDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public int EmployerID { get; set; }

            // Navigation property for the related Employer entity
            public Employer Employer { get; set; }
            public ICollection<Application> Applications { get; set; }

            public ICollection<JobSkill> JobSkills { get; set; }




    }
}
