using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace Job_Portal_API.Services
{
    public class JobListingService : IJobListing
    {
        private readonly IRepository<int, JobListing> _jobListingRepository;
        private readonly IRepository<int, Employer> _employerRepository;

        public JobListingService(IRepository<int, JobListing> jobListingRepository, IRepository<int, Employer> employerRepository)
        {
            _jobListingRepository = jobListingRepository;
            _employerRepository = employerRepository;
        }

        public JobListing MapDtoToJobListing(JobListingDTO jobListingDto)
        {  
            // Validate and convert JobType
            if (!Enum.TryParse<JobType>(jobListingDto.JobType, true, out var jobType))
            {
                throw new ArgumentException("Invalid Job type");
            }
            return new JobListing
            {
                JobTitle = jobListingDto.JobTitle,
                JobDescription = jobListingDto.JobDescription,
                JobType = jobType,
                Location = jobListingDto.Location,
                Salary = jobListingDto.Salary,
                PostingDate = jobListingDto.PostingDate,
                ClosingDate = jobListingDto.ClosingDate,
                EmployerID = jobListingDto.EmployerID,
                JobSkills = jobListingDto.Skills.Select(skill => new JobSkill { SkillName = skill.SkillName }).ToList()
            };
        }
        public JobListingResponseDTO MapJobListingToDto(JobListing jobListing)
        {

            var result = new JobListingResponseDTO
            {
                JobID = jobListing.JobID,
                JobTitle = jobListing.JobTitle,
                JobDescription = jobListing.JobDescription,
                JobType = jobListing.JobType.ToString(),
                Location = jobListing.Location,
                Salary = jobListing.Salary,
                PostingDate = jobListing.PostingDate,
                ClosingDate = jobListing.ClosingDate,
                EmployerID = jobListing.EmployerID,
                Skills = jobListing.JobSkills.Select(skill => new JobSkillDTO { SkillName = skill.SkillName }).ToList()
            };
            return result;
        }
        public async Task<JobListingResponseDTO> AddJobListingAsync(JobListingDTO jobListingDto)
        {
            try
            {
                var employer = await _employerRepository.GetById(jobListingDto.EmployerID);
                var jobListing = MapDtoToJobListing(jobListingDto);
              
                var result = await _jobListingRepository.Add(jobListing);
                var response = MapJobListingToDto(result);
                return response;    

            }
            catch (UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }


        }

        public async Task<IEnumerable<JobListingResponseDTO>> GetAllJobListingsAsync()
        {
            var jobListings = await _jobListingRepository.GetAll();
            if (jobListings.Count() == 0)
            {
                throw new NoJobExistException("No Job Listings Exist");
            }

            return jobListings.Select(jobListing => new JobListingResponseDTO
            {
                JobID = jobListing.JobID,
                JobTitle = jobListing.JobTitle,
                JobDescription = jobListing.JobDescription,
                JobType = jobListing.JobType.ToString(),
                Location = jobListing.Location,
                Salary = jobListing.Salary,
                PostingDate = jobListing.PostingDate,
                ClosingDate = jobListing.ClosingDate,
                EmployerID = jobListing.EmployerID,
                Skills = jobListing.JobSkills.Select(skill => new JobSkillDTO { SkillName = skill.SkillName }).ToList()
            });
        }

        public async Task<JobListingResponseDTO> GetJobListingByIdAsync(int id)
        {
            try
            {
                var jobListing = await _jobListingRepository.GetById(id);
                return new JobListingResponseDTO
                {
                    JobID = jobListing.JobID,
                    JobTitle = jobListing.JobTitle,
                    JobDescription = jobListing.JobDescription,
                    JobType = jobListing.JobType.ToString(),
                    Location = jobListing.Location,
                    Salary = jobListing.Salary,
                    PostingDate = jobListing.PostingDate,
                    ClosingDate = jobListing.ClosingDate,
                    EmployerID = jobListing.EmployerID,
                    Skills = jobListing.JobSkills.Select(skill => new JobSkillDTO { SkillName = skill.SkillName }).ToList()
                };
            }
            catch(JobListingNotFoundException e)
            {
                throw new JobListingNotFoundException(e.Message);
            }


            
        }

        public async  Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByEmployerIdAsync(int id)
        {
            try
            {
                var employerExists = await _employerRepository.GetById(id);
                var jobListings = await _jobListingRepository.GetAll();
                if(jobListings.Count() == 0)
                {
                    throw new NoJobExistException("No Job Listings Exist");
                }
                var employerJobListings = jobListings.Where(jl => jl.EmployerID == id);
                return employerJobListings.Select(jobListing => new JobListingResponseDTO
                {
                    JobID = jobListing.JobID,
                    JobTitle = jobListing.JobTitle,
                    JobDescription = jobListing.JobDescription,
                    JobType = jobListing.JobType.ToString(),
                    Location = jobListing.Location,
                    Salary = jobListing.Salary,
                    PostingDate = jobListing.PostingDate,
                    ClosingDate = jobListing.ClosingDate,
                    EmployerID = jobListing.EmployerID,
                    Skills = jobListing.JobSkills.Select(skill => new JobSkillDTO { SkillName = skill.SkillName }).ToList()
                });
            }
            catch(UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
            catch(NoJobExistException e)
            {
                throw new NoJobExistException(e.Message);
            }

        }

        public async Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByLocationAsync(string location)
        {
            try
            {
                var jobListings = await  GetAllJobListingsAsync();
                
                var filteredJobListings = jobListings
                .Where(job => string.Equals(job.Location.Trim(), location.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (filteredJobListings.Count == 0)
                {
                    throw new JobListingNotFoundException("No Job Listings Found");
                }
                
                return filteredJobListings;
            }
            catch(JobListingNotFoundException e)
            {
                throw new JobListingNotFoundException(e.Message);
            }
        }

        public async  Task<IEnumerable<JobListingResponseDTO>> GetJobListingsBySalaryAsync(double sRange, double eRange)
        {
            try
            {
                var jobListings = await GetAllJobListingsAsync();
                
                var filteredJobListings = jobListings
                .Where(job => job.Salary >= sRange && job.Salary <= eRange)
                .ToList();

                // Check if any job listings are found within the specified salary range
                if (!filteredJobListings.Any())
                {
                    throw new JobListingNotFoundException($"No Job Listings Found within the salary range: {sRange} - {eRange}");
                }
                return filteredJobListings;


            }
            catch (JobListingNotFoundException e)
            {
                throw new JobListingNotFoundException(e.Message);
            }
        }

        public async  Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByTitleAsync(string title)
        {
            try
            {
                    var jobListings = await GetAllJobListingsAsync();
                    var filteredJobListings = jobListings
                    .Where(job => string.Equals(job.JobTitle.Trim(), title.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (!filteredJobListings.Any())
                {
                    throw new JobListingNotFoundException($"No Job Listing Found for the Given JobTitle {title}");
                }
                return filteredJobListings;

            }
            catch (JobListingNotFoundException e)
            {
                throw new JobListingNotFoundException(e.Message);
            }
        }

        public async  Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByJobTypeAsync(string jobType)
        {    // Validate and convert JobType
            if (!Enum.TryParse<JobType>(jobType, true, out var newJobType))
            {
                throw new InvalidJobTypeException("Invalid Job type");
            }
            try
            {
                     var jobListings = await GetAllJobListingsAsync();
                var filteredJobListings = jobListings
                    .Where(job => string.Equals(job.JobType.ToString().Trim(), jobType.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                if(filteredJobListings.Count == 0)
                {
                    throw new JobListingNotFoundException($"No Job Listings Found for the Given JobType {jobType}");
                }
                return filteredJobListings;
            }
            catch (JobListingNotFoundException e)
            {
                throw new JobListingNotFoundException(e.Message);
            }
        }
    }
}
