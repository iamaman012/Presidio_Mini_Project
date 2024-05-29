using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IJobSeeker
    {
        public Task<ExperienceResponseDTO> AddExperience(ExperienceDTO experienceDTO);
        public Task<EducationResponseDTO> AddEducation(EducationDTO educationDTO);
        public Task<JobSeekerResponseDTO> GetResumeByJobSeekerId(int jobSeekerID);

        public Task<IEnumerable<JobSeekerSkillResponseDTO>> AddJobSeekerSkillsAsync(JobSeekerSkillDTO jobSeekerSkillDto);
    }
}
