using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IUser
    {
        public Task<ReturnUserDTO> RegisterUser(RegisterUserDTO user);

        public Task<ReturnLoginDTO> LoginUser(LoginUserDTO user);
        public Task<User> UpdateUserEmail(int id,String email);
        public Task<User> DeleteUserById(int id);

        public Task<User> GetUserById(int id);

        public Task<IEnumerable<User>> GetAllUsers();
    }
}
