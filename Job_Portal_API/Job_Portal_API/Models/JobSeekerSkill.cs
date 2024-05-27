namespace Job_Portal_API.Models
{
    public class JobSeekerSkill
    {
        public int JobSeekerID { get; set; }
        public int SkillID { get; set; }

        // Navigation properties
        public JobSeeker JobSeeker { get; set; }
        public Skill Skill { get; set; }
    }
}
