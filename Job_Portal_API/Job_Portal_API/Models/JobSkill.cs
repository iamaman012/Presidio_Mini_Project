namespace Job_Portal_API.Models
{
    public class JobSkill
    {
        public int JobID { get; set; }
        public int SkillID { get; set; }

        // Navigation properties
        public JobListing JobListing { get; set; }
        public Skill Skill { get; set; }
    }
}
