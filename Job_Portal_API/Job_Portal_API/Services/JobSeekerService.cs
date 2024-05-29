using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Services
{
    public class JobSeekerService : IJobSeeker
    {
        private readonly IRepository<int,JobSeeker> _jobSeekerRepo;
        private readonly IRepository<int, JobSeekerExperience> _experienceRepository;
        private readonly IRepository<int, JobSeekerEducation> _educationRepository;
        private readonly IRangeRepository<int, JobSeekerSkill> _jobSeekerSkillRepository;

        public JobSeekerService(IRepository<int, JobSeeker> jobSeekerRepo, IRepository<int, JobSeekerExperience> experienceRepository, IRepository<int, JobSeekerEducation> educationRepository, IRangeRepository<int, JobSeekerSkill> jobSeekerSkillRepository)
        {
            _jobSeekerRepo = jobSeekerRepo;
            _experienceRepository = experienceRepository;
            _educationRepository = educationRepository;
            _jobSeekerSkillRepository = jobSeekerSkillRepository;
        }

        public async Task<ExperienceResponseDTO> AddExperience(ExperienceDTO experienceDTO)
        {
            try
            {
                var jobSeeker = await _jobSeekerRepo.GetById(experienceDTO.JobSeekerID);
                var experience = new JobSeekerExperience
                {
                    JobSeekerID = experienceDTO.JobSeekerID,
                    JobTitle = experienceDTO.JobTitle,
                    CompanyName = experienceDTO.CompanyName,
                    Location = experienceDTO.Location,
                    StartDate = experienceDTO.StartDate,
                    EndDate = experienceDTO.EndDate,
                    Description = experienceDTO.Description
                };
                var addedExperience = await _experienceRepository.Add(experience);
                return new ExperienceResponseDTO
                {
                    ExperienceID = addedExperience.ExperienceID,
                    JobSeekerID = addedExperience.JobSeekerID,
                    JobTitle = addedExperience.JobTitle,
                    CompanyName = addedExperience.CompanyName,
                    Location = addedExperience.Location,
                    StartDate = addedExperience.StartDate,
                    EndDate = addedExperience.EndDate,
                    Description = addedExperience.Description
                };
            }
            catch(JobSeekerNotFoundException e)
            {
                throw new JobSeekerNotFoundException(e.Message);
            }


        }

        public async Task<EducationResponseDTO> AddEducation(EducationDTO educationDTO)
        {
            try
            {
                var jobSeeker = await _jobSeekerRepo.GetById(educationDTO.JobSeekerID);
                var education = new JobSeekerEducation
                {
                    JobSeekerID = educationDTO.JobSeekerID,
                    Degree = educationDTO.Degree,
                    Institution = educationDTO.Institution,
                    Location = educationDTO.Location,
                    StartDate = educationDTO.StartDate,
                    EndDate = educationDTO.EndDate,
                    Description = educationDTO.Description,
                    GPA = educationDTO.GPA
                };
                var addedEducation = await _educationRepository.Add(education);
                return new EducationResponseDTO
                {
                    EducationID = addedEducation.EducationID,
                    JobSeekerID = addedEducation.JobSeekerID,
                    Degree = addedEducation.Degree,
                    Institution = addedEducation.Institution,
                    Location = addedEducation.Location,
                    StartDate = addedEducation.StartDate,
                    EndDate = addedEducation.EndDate,
                    Description = addedEducation.Description,
                    GPA = addedEducation.GPA
                };
            }
            catch (JobSeekerNotFoundException e)
            {
                throw new JobSeekerNotFoundException(e.Message);
            }
  
        }

        public async Task<JobSeekerResponseDTO> GetResumeByJobSeekerId(int jobSeekerID)
        {

            try
            {
                var jobSeeker = await _jobSeekerRepo.GetById(jobSeekerID);
                return MapToDTO(jobSeeker);
            }
            catch (JobSeekerNotFoundException e) { throw new JobSeekerNotFoundException(e.Message); }
            

        }
        private JobSeekerResponseDTO MapToDTO(JobSeeker jobSeeker)
        {
            return new JobSeekerResponseDTO
            {
                JobSeekerID = jobSeeker.JobSeekerID,
                UserID = jobSeeker.UserID,
               
                Skills = jobSeeker.JobSeekerSkills.Select(skill => new JobSeekerSkillResponseDTO { SkillName = skill.SkillName }).ToList(),
                Educations = jobSeeker.JobSeekerEducations.Select(edu => new EducationResponseDTO
                {
                    EducationID = edu.EducationID,
                    JobSeekerID = edu.JobSeekerID,
                    Degree = edu.Degree,
                    Institution = edu.Institution,
                    Location = edu.Location,
                    StartDate = edu.StartDate,
                    EndDate = edu.EndDate,
                    Description = edu.Description,
                    GPA = edu.GPA
                }).ToList(),
                Experiences = jobSeeker.JobSeekerExperiences.Select(exp => new ExperienceResponseDTO
                {
                    ExperienceID = exp.ExperienceID,
                    JobTitle = exp.JobTitle,
                    CompanyName = exp.CompanyName,
                    Location = exp.Location,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    Description = exp.Description
                }).ToList()
            };
        }

        public async Task<IEnumerable<JobSeekerSkillResponseDTO>> AddJobSeekerSkillsAsync(JobSeekerSkillDTO jobSeekerSkillDto)
        {

            try
            {
                var jobSeeker = await _jobSeekerRepo.GetById(jobSeekerSkillDto.JobSeekerID);
                var jobSeekerSkills = jobSeekerSkillDto.SkillNames.Select(skillName => new JobSeekerSkill
                {
                    JobSeekerID = jobSeekerSkillDto.JobSeekerID,
                    SkillName = skillName
                });
                var addedJobSeekerSkills = await _jobSeekerSkillRepository.AddRange(jobSeekerSkills);

                return addedJobSeekerSkills.Select(skill => new JobSeekerSkillResponseDTO
                {

                    SkillName = skill.SkillName
                });
            }
            catch(JobSeekerNotFoundException e)
            {
                throw new JobSeekerNotFoundException(e.Message);
            }
       
        }
    }
}
