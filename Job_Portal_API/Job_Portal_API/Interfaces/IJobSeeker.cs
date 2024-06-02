using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IJobSeeker
    {
        public Task<ExperienceResponseDTO> AddExperience(ExperienceDTO experienceDTO);
        public Task<EducationResponseDTO> AddEducation(EducationDTO educationDTO);
        public Task<JobSeekerResponseDTO> GetResumeByJobSeekerId(int jobSeekerID);

        public Task<IEnumerable<JobSeekerSkillResponseDTO>> AddJobSeekerSkillsAsync(JobSeekerSkillDTO jobSeekerSkillDto);
        public Task<ExperienceResponseDTO> DeleteExperirnceById(int experienceId);
        public Task<EducationResponseDTO> DeleteEducationById(int educationId);
        public Task<JobSeekerSkillResponseDTO> DeleteSkillById(int skillId);
        public Task<EducationResponseDTO> UpdateEducation(EducationResponseDTO educationDTO);
        public Task<ExperienceResponseDTO> UpdateExperience(ExperienceResponseDTO experienceDTO);
        public Task<UpdateSkillDTO> UpdateSkill(int jobSeekerId,int skillId,string skillName);

    }
}
