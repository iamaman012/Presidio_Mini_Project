using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using System.Xml.Linq;

namespace Job_Portal_API.Services
{
    public class ApplicationService : IApplication
    {   
        private readonly IRepository<int, Application> _applicationRepository;
        private readonly IRepository<int, JobListing> _jobListingRepository;
        private readonly IRepository<int, JobSeeker> _jobSeekerRepository;
        private readonly EmailService _emailService;

        public ApplicationService(
            IRepository<int, Application> applicationRepository,
            IRepository<int, JobListing> jobListingRepository,
            IRepository<int, JobSeeker> jobSeekerRepository,
            EmailService emailService)
        {
            _applicationRepository = applicationRepository;
            _jobListingRepository = jobListingRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _emailService = emailService;
        }
        public async  Task<ApplicationResponseDTO> SubmitApplication(int jobID, int jobSeekerID)
        {

            try
            {
                var applications = await _applicationRepository.GetAll();
                var existApplication = applications.FirstOrDefault(ap => ap.JobSeekerID == jobSeekerID && ap.JobID == jobID);
                if (existApplication != null) throw new ApplicationAlreadyExistException("Application Already Exist");
                var jobListing = await _jobListingRepository.GetById(jobID);
                var jobSeeker = await _jobSeekerRepository.GetById(jobSeekerID);
                var application = new Application
                {
                    JobID = jobID,
                    JobSeekerID = jobSeekerID,
                    Status = "Pending"
                };

                var addedApplication = await _applicationRepository.Add(application);
                
                var email = jobSeeker.User.Email;
                var name = jobSeeker.User.FirstName + " " + jobSeeker.User.LastName;
              
               
                var jobTitle = jobListing.JobTitle;
                var companyName = jobListing.CompanyName;
                // Send notification email
                string subject = $"Applied Successfully";
                string body = $"Dear {name},\n\nYour Have Successfully Applied for the Job , Our people will review your application and notify you shortly.\nJobID :{jobID}\nJob Title : {jobTitle}\nCompany : {companyName}\n\nBest regards,\nJob Entry";
                _emailService.SendEmail(email, subject, body);
                return await MapToDTO(addedApplication);

            }
            catch (JobListingNotFoundException e) {
                throw new JobListingNotFoundException(e.Message);
            }
            catch (JobSeekerNotFoundException e)
            {
                throw new JobSeekerNotFoundException(e.Message);
            }
            catch(ApplicationAlreadyExistException e)
            {
                throw new ApplicationAlreadyExistException(e.Message);
            }


            
           
           
        }
        public async Task<ApplicationResponseDTO> MapToDTO(Application application)
        {
            return new ApplicationResponseDTO
            {
                ApplicationID = application.ApplicationID,
                JobID = application.JobID,
                JobSeekerID = application.JobSeekerID,
                Status = application.Status,
                ApplicationDate = application.ApplicationDate
            };
        }

        public async  Task<ApplicationStatusDTO> GetApplicationStatus(int applicationId)
        {
            try
            {
                var application = await _applicationRepository.GetById(applicationId);
                var result = new ApplicationStatusDTO
                {
                    ApplicationID = application.ApplicationID,
              
                    Status = application.Status
                };
                return result;
            }
            catch(ApplicationNotFoundException e)
            {
                throw new ApplicationNotFoundException(e.Message);
            }
        }
        public async Task<IEnumerable<ApplicationResponseDTO>> GetApplicationByJobSeekerID(int jobSeekerID)
        {
            try
            {
                var jobSeeker = await _jobSeekerRepository.GetById(jobSeekerID);
                var applications = await _applicationRepository.GetAll();
                if(!applications.Any()) throw new ApplicationNotFoundException();
                var filteredApplications = applications.Where(application => application.JobSeekerID == jobSeekerID);
                if(!filteredApplications.Any()) throw new ApplicationNotFoundException($"No Application Found for the Given JobSeekerID {jobSeekerID}");
                return filteredApplications.Select(application => new ApplicationResponseDTO
                {
                    ApplicationID = application.ApplicationID,
                    JobID = application.JobID,
                    JobSeekerID = application.JobSeekerID,
                    Status = application.Status,
                    ApplicationDate = application.ApplicationDate,
                    CompanyImage = application.JobListing.ImageUrl,
                    JobTitle = application.JobListing.JobTitle,
                    JobType = application.JobListing.JobType.ToString(),
                    Location = application.JobListing.Location,
                    Salary = application.JobListing.Salary,
                    JobSeekerName = jobSeeker.User.FirstName + " " + jobSeeker.User.LastName,
                   



                });
            }
            catch (JobSeekerNotFoundException e) {
                throw new JobSeekerNotFoundException(e.Message);
            }
            catch (ApplicationNotFoundException e)
            {
                throw new ApplicationNotFoundException(e.Message);
            }

        }

        public async Task<ApplicationResponseDTO> DeleteApplicationById(int applicationId)
        {
            try
            {
                var application = await _applicationRepository.DeleteById(applicationId);
                return new ApplicationResponseDTO
                {
                    ApplicationID = application.ApplicationID,
                    JobID = application.JobID,
                    JobSeekerID = application.JobSeekerID,
                    Status = application.Status
                };
            }
            catch (ApplicationNotFoundException e)
            {
                throw new ApplicationNotFoundException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
