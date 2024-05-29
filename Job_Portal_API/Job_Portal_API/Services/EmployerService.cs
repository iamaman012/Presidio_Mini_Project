using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;

namespace Job_Portal_API.Services
{
    public class EmployerService : IEmployer
    {
        private readonly IRepository<int, Employer> _repository;
        private readonly IRepository<int, User> _UserRepo;

        public EmployerService(IRepository<int,Employer> repository,IRepository<int,User> userRepo) { 
                _repository= repository;
                _UserRepo = userRepo;
        }
        public async Task<ReturnEmployerDTO> AddEmployerDetails(AddEmployerDTO employer)
        {
            try
            {    var user = await _UserRepo.GetById(employer.UserID);
                if(user.UserType!=UserType.Employer)
                {
                    throw new UserTypeNotAllowedException();
                }
                var newEmployer = new Employer
                {
                    UserID = employer.UserID,
                    CompanyName = employer.CompanyName,
                    CompanyDescription = employer.CompanyDescription,
                    CompanyLocation = employer.CompanyLocation
                };
                var result = await _repository.Add(newEmployer);
                return await MapEmployeroDTO(result);
            }
            catch (UserNotFoundException e)
            {

                throw new UserNotFoundException(e.Message);
            }
            catch (UserAlreadyExistException e)
            {
                throw new UserAlreadyExistException(e.Message);
            }
            catch (UserTypeNotAllowedException e)
            {
                throw new UserTypeNotAllowedException(e.Message);
            }

        }

        public async Task<ReturnEmployerDTO> UpdateCompanyDescription(int id, string companyDescription)
        {
            try
            {
                var employer = await _repository.GetById(id);
                employer.CompanyDescription = companyDescription;
                var result = await _repository.Update(employer);
                return await MapEmployeroDTO(result);
            }
            catch (UserNotFoundException e)
            {

                throw new UserNotFoundException("Employer Not Exist!!");
            }
        }

        public async Task<ReturnEmployerDTO> UpdateCompanyLocation(int id, string companyLocation)
        {
            try
            {
                var employer = await _repository.GetById(id);
                employer.CompanyLocation = companyLocation;
                var result = await _repository.Update(employer);
                return await MapEmployeroDTO(result);
            }
            catch(UserNotFoundException e)
            {
                throw new UserNotFoundException("Employer Not Exist!!");
            }
        }

        public async Task<ReturnEmployerDTO> UpdateCompanyName(int id, string companyName)
        {
            try
            {
                var employer = await _repository.GetById(id);
                employer.CompanyName = companyName;
                var result = await _repository.Update(employer);
                return await MapEmployeroDTO(result);
            }
            catch (UserNotFoundException e)
            {

                throw new UserNotFoundException("Employer Not Exist!!");
            }
        }
        public async Task<ReturnEmployerDTO> MapEmployeroDTO(Employer employer)
        {
            return new ReturnEmployerDTO
            {
                EmployerID = employer.EmployerID,
                UserID = employer.UserID,
                CompanyName = employer.CompanyName,
                CompanyDescription = employer.CompanyDescription,
                CompanyLocation = employer.CompanyLocation
            };
        }
    }
}
