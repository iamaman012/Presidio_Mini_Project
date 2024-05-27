namespace Job_Portal_API.Models
{
    public class JobSeeker
    {
        
        
            public int JobSeekerID { get; set; }
            public int UserID { get; set; }

            // Navigation property for the User
            public User User { get; set; }

            public ICollection<Application> Applications { get; set; }
            public ICollection<JobSeekerSkill> JobSeekerSkills { get; set; }
            
            public ICollection<JobSeekerEducation> JobSeekerEducations { get; set; }

            public ICollection<JobSeekerExperience> JobSeekerExperiences { get; set; }






    }
}
