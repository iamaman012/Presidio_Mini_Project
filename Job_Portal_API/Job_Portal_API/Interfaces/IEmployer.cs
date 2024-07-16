using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IEmployer
    {
        public Task<ReturnEmployerDTO> AddEmployerDetails(AddEmployerDTO employer);
        public Task<ReturnEmployerDTO> GetEmployerById(int id);
        public Task<ReturnEmployerDTO> UpdateCompanyName(int id, String companyName);
        public Task<ReturnEmployerDTO> UpdateCompanyDescription(int id, String companyDescription);

        public Task<ReturnEmployerDTO> UpdateCompanyLocation(int id, String companyLocation);

        public Task<ReturnEmployerDTO> UpdateEmployer(UpdateEmployerDTO updateEmployerDTO);
    }
}
