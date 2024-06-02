using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IAdmin
    {
        public Task<IEnumerable<UserAdminResponseDTO>> GetAllUsers();
        public Task<IEnumerable<JobSeekerResponseDTO>> GetAllJobSeekers();
        public Task<IEnumerable<ReturnEmployerDTO>> GetAllEmployers();
        public Task<IEnumerable<JobListingResponseDTO>> GetAllJobListings();
        public Task<IEnumerable<ApplicationResponseDTO>> GetAllApplications();
    }
}
