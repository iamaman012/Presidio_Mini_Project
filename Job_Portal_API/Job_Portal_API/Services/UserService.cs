using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Job_Portal_API.Services
{
    public class UserService : IUser
    {
        private readonly IRepository<int, User> _repository;
        private readonly IToken _tokenService;
        private readonly IRepository<int, JobSeeker> _jobSeekerRepo;

        public UserService(IRepository<int,User> repository,IToken tokenService, IRepository<int, JobSeeker> jobSeekerRepo)
        {
            _repository = repository;
            _tokenService = tokenService;
            _jobSeekerRepo = jobSeekerRepo;
        }
        public async Task<ReturnUserDTO> RegisterUser(RegisterUserDTO userDTO)
        {
            try
            {
                User user = MapRegisterUserDTOToUser(userDTO); 
                if(user.UserType == UserType.JobSeeker)
                user.JobSeeker = new JobSeeker();
                var result= await _repository.Add(user);
                

                ReturnUserDTO returnUser = new ReturnUserDTO {UserID=result.UserID, Email = result.Email,Role = result.UserType.ToString(),Name=result.FirstName+result.LastName,ContactNumber=result.ContactNumber,JobSeekerID=result.JobSeeker?.JobSeekerID };
              
                return returnUser;
            }
            catch(UserAlreadyExistException e)
            {
                throw new UserAlreadyExistException(e.Message);
            }
            catch(ArgumentException e)
            {
                throw new ArgumentException("Please Enter Valid User Type");
            }
        }
        public async Task<ReturnLoginDTO> LoginUser(LoginUserDTO userDTO)
        {
            try
            {
                var users = await _repository.GetAll();
                var user = users.FirstOrDefault(u => u.Email == userDTO.Email);
                if (user == null)
                {
                    throw new UnauthorizedUserException("Invalid Email or password");
                }
                HMACSHA512 hMACSHA = new HMACSHA512(user.HashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, user.Password);
                if (isPasswordSame)
                {


                    ReturnLoginDTO loginReturnDTO = MapUserToLoginReturn(user);
                    return loginReturnDTO;



                }
                throw new UnauthorizedUserException("Invalid username or password");
            }
            catch(UnauthorizedUserException e)
            {
                throw new UnauthorizedUserException(e.Message);
            }
           
            
        }

        public async Task<ReturnUserDTO> DeleteUserById(int id)
        {
            try
            {
                var user = await _repository.DeleteById(id);
                ReturnUserDTO returnUser = new ReturnUserDTO { UserID = user.UserID, Email = user.Email, Role = user.UserType.ToString(), Name = user.FirstName + " "+ user.LastName, ContactNumber = user.ContactNumber,JobSeekerID=user.JobSeeker?.JobSeekerID };
                return returnUser;
            }
            catch (UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
        }

       

        public async Task<ReturnUserDTO> GetUserById(int id)
        {
            try
            {
                var user = await _repository.GetById(id);
                ReturnUserDTO returnUser = new ReturnUserDTO { UserID = user.UserID, Email = user.Email, Role = user.UserType.ToString(), Name = user.FirstName + user.LastName, ContactNumber = user.ContactNumber,JobSeekerID=user.JobSeeker?.JobSeekerID};
                return returnUser;
            }
            catch (UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
        }

        public async Task<ReturnUserDTO> UpdateUserEmail(int id, string email)
            {
                try
                {
                    var user = await _repository.GetById(id);
                    user.Email = email;
                    user = await _repository.Update(user);
                ReturnUserDTO returnUser = new ReturnUserDTO() { UserID = user.UserID, Email = user.Email, Role = user.UserType.ToString(), Name = user.FirstName + user.LastName, ContactNumber = user.ContactNumber };
                return returnUser;

            }
                catch (UserNotFoundException e)
                {
                    throw new UserNotFoundException(e.Message);
                }
            }
        private User MapRegisterUserDTOToUser(RegisterUserDTO userDTO)
        {
            // Validate and convert UserType
            if (!Enum.TryParse<UserType>(userDTO.UserType, true, out var userType))
            {
                throw new ArgumentException("Invalid user type");
            }
            User user = new User()
            {
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserType = userType,
                ContactNumber = userDTO.ContactNumber,
                
            };
            
            
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.HashKey = hMACSHA.Key;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
            return user;
        }
        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }
        private ReturnLoginDTO MapUserToLoginReturn(User user)
        {
            ReturnLoginDTO returnDTO = new ReturnLoginDTO();
            returnDTO.UserID = user.UserID;
            returnDTO.Role = user.UserType.ToString();
            returnDTO.Email = user.Email;
            returnDTO.Token = _tokenService.GenerateJSONWebToken(user);
            return returnDTO;
        }

        

        public  async Task<string> ChangePassword(int id, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if(newPassword != confirmPassword)
                {
                    throw new ArgumentException("New Password and Confirm Password does not match");
                }
                var user = await _repository.GetById(id);
                HMACSHA512 hMACSHA = new HMACSHA512(user.HashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
                bool isPasswordSame = ComparePassword(encrypterPass, user.Password);
                if (isPasswordSame)
                {
                    user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                    user = await _repository.Update(user);
                    return "Password Changed Successfully";
                }
                throw new UnauthorizedUserException("Invalid Password");

            }
            catch(ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch(UserNotFoundException e)
            {
                throw new UserNotFoundException(e.Message);
            }
            catch (UnauthorizedUserException e)
            {
                throw new UnauthorizedUserException(e.Message);
            }
        }
    }
}
