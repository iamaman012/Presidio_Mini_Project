using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;

        public UserController(IUser service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ReturnUserDTO>>Register(RegisterUserDTO user)
        {
            if (ModelState.IsValid)
            {   
                try
                {
                    var result = await _service.RegisterUser(user);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status409Conflict, e.Message);
                }
            }
            return BadRequest("All fields are required!!");
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ReturnLoginDTO>> Login(LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.LoginUser(userDTO);
                    return Ok(result);
                }
                catch (UserNotFoundException e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, e.Message);
                }
            }
            return BadRequest("All fields are required!!");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<ReturnUserDTO>>> GetAllUsers()
        {
            try
            {
                var result = await _service.GetAllUsers();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Authorize]
        [Authorize(Roles = "Employer,Admin,JobSeeker")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<ReturnUserDTO>> DeleteUser(int id)
        {
            try
            {
                var result = await _service.DeleteUserById(id);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [Authorize(Roles = "Employer,Admin,JobSeeker")]
        [HttpPost("GetUserById")]
        public async Task<ActionResult<ReturnUserDTO>> GetUserById(int id)
        {
            try
            {
                var result = await _service.GetUserById(id);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [Authorize]
        [HttpPut("UpdateUserEmail")]
        public async Task<ActionResult<ReturnUserDTO>> UpdateUserEmail(int id, string email)
        {
            try
            {
                var result = await _service.UpdateUserEmail(id, email);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
