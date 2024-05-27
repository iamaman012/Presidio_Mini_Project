using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Job_Portal_API.Services
{
    public class UserService : IUser
    {
        private readonly IRepository<int, User> _repository;
        private readonly IToken _tokenService;

        public UserService(IRepository<int,User> repository,IToken tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }
        public async Task<ReturnUserDTO> RegisterUser(RegisterUserDTO userDTO)
        {
            try
            {
                User user = MapRegisterUserDTOToUser(userDTO);
                var result= await _repository.Add(user);
                ReturnUserDTO returnUser = new ReturnUserDTO() {UserID=result.UserID, Email = result.Email,Role = result.UserType };
                return returnUser;
            }
            catch(Exception e)
            {
                throw new UserAlreadyExistException();
            }
        }
        public async Task<ReturnLoginDTO> LoginUser(LoginUserDTO userDTO)
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

        public async Task<User> DeleteUserById(int id)
        {
            try
            {
                return await  _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _repository.GetAll();
            
            if (users.Count() == 0)
            {
                throw new NoUsersFoundException();
            }
            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                return await _repository.GetById(id);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<User> UpdateUserEmail(int id, string email)
            {
                try
                {
                    var user = await _repository.GetById(id);
                    user.Email = email;
                    user = await _repository.Update(user);
                    return user;
                }
                catch (Exception e)
                {
                    throw new UserNotFoundException();
                }
            }
        private User MapRegisterUserDTOToUser(RegisterUserDTO userDTO)
        {
            User user = new User() {Email=userDTO.Email,FirstName=userDTO.FirstName,LastName=userDTO.LastName,
            UserType=userDTO.UserType,ContactNumber=userDTO.ContactNumber};
            
            
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
            returnDTO.Role = user.UserType;
            returnDTO.Email = user.Email;
            returnDTO.Token = _tokenService.GenerateJSONWebToken(user);
            return returnDTO;
        }

        
    }
}
