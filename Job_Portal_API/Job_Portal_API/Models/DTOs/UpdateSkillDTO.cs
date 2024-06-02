namespace Job_Portal_API.Models.DTOs
{
    public class UpdateSkillDTO
    {
        public int SkillId { get; set; }   
        public int JobSeekerId { get; set; }    
        public string SkillName { get; set;}
    }

}
