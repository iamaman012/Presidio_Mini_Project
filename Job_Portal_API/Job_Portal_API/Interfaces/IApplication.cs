using Job_Portal_API.Models.DTOs;
using System.Runtime.CompilerServices;

namespace Job_Portal_API.Interfaces
{
    public interface IApplication
    {
        public Task<ApplicationResponseDTO> SubmitApplication(int jobID,int jobSeekerId);

        public Task<ApplicationStatusDTO> GetApplicationStatus(int applicationId);
        public Task<IEnumerable<ApplicationResponseDTO>> GetApplicationByJobSeekerID(int jobSeekerID);
    }
}
