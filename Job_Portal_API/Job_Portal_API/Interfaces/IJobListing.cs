using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IJobListing
    {
        public Task<JobListingResponseDTO> AddJobListingAsync(JobListingDTO jobListingDto);
        public  Task<IEnumerable<JobListingResponseDTO>> GetAllJobListingsAsync();
        public Task<JobListingResponseDTO > GetJobListingByIdAsync(int id);
        public Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByEmployerIdAsync(int employerId);

        public Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByLocationAsync(string location);

        public Task<IEnumerable<JobListingResponseDTO>> GetJobListingsBySalaryAsync(double sRange, double eRange);
        public Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByTitleAsync(string title);
        public Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByJobTypeAsync(string jobType);

        public Task<IEnumerable<ApplicationResponseDTO>> GetJobResponseByJobID(int jobID);
        public Task<ApplicationResponseDTO> UpdateApplicationStatus(int applicationId,string status);
        public Task<JobListingResponseDTO> DeleteJobListingById(int jobID);
        public Task<ReturnUserDTO> GetJobSeekerContactInformation(int jobSeekerId);
        public Task<JobSkillResponseDTO> DeleteJobSkillByID(int jobID,int skillID );
        public Task<JobListingResponseDTO> ChangeJobLocation(int jobID, string location);
        public Task<JobListingResponseDTO> ChangeJobSalary(int jobID, double salary);





    }
}
