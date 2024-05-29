namespace Job_Portal_API.Models
{
    public class JobSeekerSkill
    {   public int JobSeekerSkillID { get; set; }
        public int JobSeekerID { get; set; }
       public string SkillName { get; set; }    

        // Navigation properties
        public JobSeeker JobSeeker { get; set; }
       
    }
}
