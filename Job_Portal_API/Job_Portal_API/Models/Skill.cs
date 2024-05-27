namespace Job_Portal_API.Models
{
    public class Skill
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }

        // Navigation properties
        public ICollection<JobSeekerSkill> JobSeekerSkills { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; }
    }
}
