using Job_Portal_API.Models.Enums;
using System.ComponentModel;

namespace Job_Portal_API.Models
{
    public class JobListing
    {
        
        
            public int JobID { get; set; }
            public string JobTitle { get; set; }
            public string JobDescription { get; set; }
            public JobType JobType { get; set; }
            public string Location { get; set; }
            public double Salary { get; set; }
            public DateTime PostingDate { get; set; }= DateTime.Now;
            public DateTime ClosingDate { get; set; }
            public int EmployerID { get; set; }

            public string Category { get; set; }

            public string ImageUrl { get; set; }    

            public string CompanyName { get; set; }

            public string CompanyDescription { get; set; }

            public string CompanyLocation { get; set; }

        // Navigation property for the related Employer entity
            public Employer Employer { get; set; }
            public ICollection<Application> Applications { get; set; }

            public ICollection<JobSkill> JobSkills { get; set; }




    }
}
