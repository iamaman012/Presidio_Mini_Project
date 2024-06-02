using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Services
{
    public class AdminService : IAdmin

    {
        private readonly IRepository<int,Application> _applicationRepository;
        private readonly IRepository<int, Employer> _employerRepository;
        private readonly IJobListing _jobListingService;
        private readonly IRepository<int, JobSeeker> _jobSeekerRepository;
        private readonly IRepository<int, User> _userRepository;

        public AdminService(IRepository<int, Application> applicationRepository, IRepository<int, Employer> employerRepository, IJobListing jobListingService, IRepository<int, JobSeeker> jobSeekerRepository, IRepository<int,User> userRepository)
        {
            _applicationRepository = applicationRepository;
            _employerRepository = employerRepository;
            _jobListingService = jobListingService;
            _jobSeekerRepository = jobSeekerRepository;
            _userRepository=userRepository;
        }

        public async Task<IEnumerable<ApplicationResponseDTO>> GetAllApplications()
        {
            try
            {
                var applications = await _applicationRepository.GetAll();
                if(!applications.Any() )throw new ApplicationNotFoundException("No Applications Found");
                var applicationDTOs = applications.Select(application => new ApplicationResponseDTO
                {
                    ApplicationID = application.ApplicationID,
                    JobID = application.JobID,
                    JobSeekerID = application.JobSeekerID,
                    Status = application.Status
                });
                return applicationDTOs;
            }
            catch(ApplicationNotFoundException e)
            {
                throw new ApplicationNotFoundException(e.Message);
            }
        }

        public async Task<IEnumerable<ReturnEmployerDTO>> GetAllEmployers()
        {
            try
            {
                var employers = await _employerRepository.GetAll();
                if(!employers.Any()) throw new UserNotFoundException("No Employers Found");
                var employerDTOs = employers.Select(employer => new ReturnEmployerDTO
                {
                    EmployerID = employer.EmployerID,
                    UserID = employer.UserID,
                    CompanyName = employer.CompanyName,
                    CompanyDescription = employer.CompanyDescription,
                    CompanyLocation = employer.CompanyLocation
                });
                return employerDTOs;
            }
            catch (UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
        }

        public async Task<IEnumerable<JobListingResponseDTO>> GetAllJobListings()
        {
            try
            {
              var jobListings = await _jobListingService.GetAllJobListingsAsync();
                return jobListings;
            }
            catch (NoJobExistException e)
            {
                throw new NoJobExistException(e.Message);
            }
        }

        public async Task<IEnumerable<JobSeekerResponseDTO>> GetAllJobSeekers()
        {
            try
            {
                var jobSeekers = await _jobSeekerRepository.GetAll();
                if(!jobSeekers.Any()) throw new JobSeekerNotFoundException("No Job Seekers Found");
                var jobSeekerDTOs = jobSeekers.Select(jobSeeker => new JobSeekerResponseDTO
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
                });
                return jobSeekerDTOs;

            }
            catch (JobSeekerNotFoundException e)
            {
                throw new JobSeekerNotFoundException(e.Message);
            }
        }

        public async Task<IEnumerable<UserAdminResponseDTO>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAll();
                if (!users.Any()) throw new UserNotFoundException("No Users Found");
                var userDTOs = users.Select(user => new UserAdminResponseDTO
                {
                    UserID = user.UserID,
                    Email = user.Email,
                    Role = user.UserType.ToString(),
                    Name = user.FirstName + " " + user.LastName,
                    ContactNumber = user.ContactNumber,
                    
                });
                return userDTOs;
            }
            catch (UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
        }
    }
}
