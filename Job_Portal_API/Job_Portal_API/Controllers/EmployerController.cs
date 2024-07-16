using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployer _service;

        public EmployerController(IEmployer service)
        {
            _service = service;
        }
        [Authorize(Roles = "Employer")]
        [HttpPost("AddEmployerDetails")]
        public async Task<ActionResult<ReturnEmployerDTO>> CreateEmployerDetails(AddEmployerDTO employer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.AddEmployerDetails(employer);
                    return Ok(result);
                }
                catch (UserNotFoundException e)
                {
                    return NotFound(new ErrorModelDTO(404, e.Message));
                }
                catch (UserAlreadyExistException e)
                {
                    return BadRequest(new ErrorModelDTO(400, e.Message));
                }
                catch (UserTypeNotAllowedException e)
                {
                    return BadRequest(new ErrorModelDTO(400, e.Message));
                }
                catch(Exception e)
                {  var errorResponse = new ErrorModelDTO(500, e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }
            return BadRequest(new ErrorModelDTO(400,"All fields are Required!1"));
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("UpdateCompanyDescription")]
        public async Task<ActionResult<ReturnEmployerDTO>> UpdateCompanyDescription([Required]int employerId,[Required] string companyDescription)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateCompanyDescription(employerId, companyDescription);
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
            return BadRequest("All fields are required!!");
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("UpdateCompanyLocation")]
        public async Task<ActionResult<ReturnEmployerDTO>> UpdateCompanyLocation([Required]int emoloyerId,[Required]string companyLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateCompanyLocation(emoloyerId, companyLocation);
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
            return BadRequest("All fields are required!!");
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("GetEmployerById")]
        public async Task<ActionResult<ReturnEmployerDTO>> GetEmployerById([Required] int employerId)
        {
            try
            {
                var result = await _service.GetEmployerById(employerId);
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404,e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles ="Employer")]
        [HttpPut("UpdateCompanyDetails")]
        public async Task<ActionResult<ReturnEmployerDTO>> UpdateEmployer(UpdateEmployerDTO updateEmployerDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateEmployer(updateEmployerDTO);
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
            return BadRequest("All fields are required!!");
        }
    }
}
