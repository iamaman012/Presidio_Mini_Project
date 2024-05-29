namespace Job_Portal_API.Models
{
    public class JobSkill
    {   public int JobSkillID { get; set; }
        public int JobID { get; set; }
        public string SkillName { get; set; }   

        // Navigation properties
        public JobListing JobListing { get; set; }
        
    }
}
