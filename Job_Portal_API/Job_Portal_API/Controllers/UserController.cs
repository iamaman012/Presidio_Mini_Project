using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
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
                catch (UserAlreadyExistException e)
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
    }
}
