﻿using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;

namespace Job_Portal_API.Interfaces
{
    public interface IUser
    {
        public Task<ReturnUserDTO> RegisterUser(RegisterUserDTO userDTO);

        public Task<ReturnLoginDTO> LoginUser(LoginUserDTO user);
        public Task<ReturnUserDTO> UpdateUserEmail(int id,String email);
        public Task<ReturnUserDTO> DeleteUserById(int id);

        public Task<ReturnUserDTO> GetUserById(int id);
        public Task<string> ChangePassword(int id, string oldPassword,string newPassword,string newPassword1);


    }
}
