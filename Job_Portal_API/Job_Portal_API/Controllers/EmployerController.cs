using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Employer,Admin")]
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
                    return StatusCode(StatusCodes.Status404NotFound, e.Message);
                }
            }
            return BadRequest("All fields are required!!");
        }
        [Authorize(Roles = "Employer,Admin")]
        [HttpPut("UpdateCompanyDescription/{id}")]
        public async Task<ActionResult<ReturnEmployerDTO>> UpdateCompanyDescription(int id, string companyDescription)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateCompanyDescription(id, companyDescription);
                    return Ok(result);
                }
                catch (UserNotFoundException e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, e.Message);
                }
            }
            return BadRequest("All fields are required!!");
        }
        [Authorize(Roles = "Employer,Admin")]
        [HttpPut("UpdateCompanyLocation/{id}")]
        public async Task<ActionResult<ReturnEmployerDTO>> UpdateCompanyLocation(int id, string companyLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateCompanyLocation(id, companyLocation);
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
