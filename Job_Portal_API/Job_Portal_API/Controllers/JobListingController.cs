using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListing _service;
        private readonly IJobSeeker _jobSeekerService;
        


        public JobListingController(IJobListing service, IJobSeeker jobSeekerService)
        {
            _service = service;
            _jobSeekerService = jobSeekerService;
            
        }
        [Authorize(Roles = "Employer")]
        [HttpPost("AddJobListing")]
        public async Task<IActionResult> AddJobListing([FromBody] JobListingDTO jobListingDto)
        {
            if (jobListingDto == null)
            {
                return BadRequest(new  ErrorModelDTO(400, "Invalid Job Listing Data"));
            }

            try
            {
                var result = await _service.AddJobListingAsync(jobListingDto);
                return Ok(result);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404,e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500,e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("ByEmployerId")]


        public async Task<IActionResult> GetJobListingsByEmployerId([ Required] int employerId)
        {
            try
            {
                var jobListings = await _service.GetJobListingsByEmployerIdAsync(employerId);
                return Ok(jobListings);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404,e.Message));
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("GetApplicationByJobID")]
        public async Task<IActionResult> GetJobResponseByJobID( [Required]int jobID)
        {
            try
            {
                var jobListings = await _service.GetJobResponseByJobID(jobID);
                return Ok(jobListings);
            }
            catch (NoApplicationExistException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch(JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize]
        [HttpGet("GetJobListingByJobID")]
        public async Task<IActionResult> GetJobListingByJobID([Required] int jobID)
        {
            try
            {
                var jobListings = await _service.GetJobListingByIdAsync(jobID);
                return Ok(jobListings);
            }
            catch (NoApplicationExistException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("UpdateApplicationStatus")]
        public async Task<IActionResult> UpdateApplicationStatus([Required]int applicationId,[Required]string status)
        {
            try
            {
                var response = await _service.UpdateApplicationStatus(applicationId,status);
               
                return Ok(response);
            }
            catch (ApplicationNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("ReviewJobSeekerResume")]
        public async Task<IActionResult> ReviewJobSeekerResume([Required]int jobSeekerID)
        {
            try
            {
                var response = await _jobSeekerService.GetResumeByJobSeekerId(jobSeekerID);
                return Ok(response);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpDelete("DeleteJobListingById")]
        public async Task<IActionResult> DeleteJobListingById([Required]int jobID)
        {
            try
            {
                var response = await _service.DeleteJobListingById(jobID);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("JobSeekerContactDetails")]
        public async Task<IActionResult> GetJobSeekerContactDetails([Required]int jobSeekerID)
        {
            try
            {
                var response = await _service.GetJobSeekerContactInformation(jobSeekerID);
                return Ok(response);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpDelete("DeleteJobSkillByID")]
        public async Task<IActionResult> DeleteJobSkillByID([Required] int jobID, [Required] int skillID)
        {
            try
            {
                var response = await _service.DeleteJobSkillByID(jobID, skillID);
                return Ok(response);
            }
            catch (JobSkillNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [Authorize(Roles ="Employer")]
        [HttpPut("ChangeJobLocation")]
        public async Task<IActionResult> ChangeJobLocation([Required] int jobID, [Required] string location)
        {
            try
            {
                var response = await _service.ChangeJobLocation(jobID, location);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "Employer")]
        [HttpPut("ChangeJobSalary")]
        public async Task<IActionResult> ChangeJobSalary([Required] int jobID, [Required] double salary)
        {
            try
            {
                var response = await _service.ChangeJobSalary(jobID, salary);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
