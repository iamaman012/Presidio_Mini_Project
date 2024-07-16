namespace Job_Portal_API.Models
{
    public class Employer
    {
        
        
            public int EmployerID { get; set; }
            public int UserID { get; set; }
            public string ?CompanyName { get; set; }
            public string? CompanyDescription { get; set; }
            public string? CompanyLocation { get; set; }

            // Navigation property for the User
            public User User { get; set; }
            public ICollection<JobListing> JobListings { get; set; }


    }
}
