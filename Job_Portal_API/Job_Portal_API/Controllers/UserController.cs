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
        public async Task<IActionResult>Register(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {   
                try
                {
                    var result = await _service.RegisterUser(userDTO);
                   
                    return Ok(result);
                }
                catch (UserAlreadyExistException e)
                {
                    return Conflict(new ErrorModelDTO(409,e.Message));
                }
                catch (ArgumentException e)
                {
                    return BadRequest(new ErrorModelDTO(400,e.Message));
                }
                catch (Exception e)
                {   var errorResponse = new ErrorModelDTO(500, e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }
            return BadRequest(new ErrorModelDTO(400,"All Fields are Required"));
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
                catch (UnauthorizedUserException e)
                {
                    return NotFound(new ErrorModelDTO(404, e.Message));
                }
                catch (Exception e)
                {
                    var errorResponse = new ErrorModelDTO(500, e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }
            return BadRequest(new ErrorModelDTO(400, "All Fields are Required"));
        }
       
       
        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult<ReturnUserDTO>> DeleteUser(int userid)
        {
            try
            {
                var result = await _service.DeleteUserById(userid);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
       
        [HttpGet("GetUserById")]
        public async Task<ActionResult<ReturnUserDTO>> GetUserById(int userid)
        {
            try
            {
                var result = await _service.GetUserById(userid);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        
        [HttpPut("UpdateUserEmailByID")]
        public async Task<ActionResult<ReturnUserDTO>> UpdateUserEmail(int userid, string email)
        {
            try
            {
                var result = await _service.UpdateUserEmail(userid, email);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpPut("ChangePassword")]
        public async Task<ActionResult<ReturnUserDTO>> ChangePassword(int userid, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                var result = await _service.ChangePassword(userid, oldPassword, newPassword, confirmPassword);

                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorModelDTO(400, e.Message));
            }
            catch (UnauthorizedUserException e)
            {
                return Unauthorized(new ErrorModelDTO(401, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
