namespace Job_Portal_API.Models.DTOs
{
    public class JobSeekerResponseDTO
    {
        public int JobSeekerID { get; set; }
        public int UserID { get; set; }
        public ICollection<JobSeekerSkillResponseDTO> Skills { get; set; }
        public ICollection<EducationResponseDTO> Educations { get; set; }
        public ICollection<ExperienceResponseDTO > Experiences { get; set; }
    }
}
