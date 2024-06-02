using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Employer,Admin")]
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
        //[HttpGet("GetAllJobListings")]
        //public async Task<IActionResult> GetAllJobListings()
        //{
        //    try
        //    {
        //        var jobListings = await _service.GetAllJobListingsAsync();
        //        return Ok(jobListings);
        //    }
        //    catch (JobListingNotFoundException e)
        //    {
        //        return NotFound(new ErrorModelDTO(404,e.Message));
        //    }
        //    catch (Exception e)
        //    {
        //        var errorResponse = new ErrorModelDTO(500, e.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        //    }
        //}
        [HttpGet("ByEmployerId")]

        public async Task<IActionResult> GetJobListingsByEmployerId(int employerId)
        {
            try
            {
                var jobListings = await _service.GetJobListingsByEmployerIdAsync(employerId);
                return Ok(jobListings);
            }
            catch (UserNotFoundException e)
            {
                return NotFound("Employer not found.");
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound("No job listings found.");
            }
        }

        [HttpGet("GetApplicationByJobID")]
        public async Task<IActionResult> GetJobResponseByJobID( int jobID)
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
        [HttpPut("UpdateApplicationStatus")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId,string status)
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
        [HttpGet("ReviewJobSeekerResume")]
        public async Task<IActionResult> ReviewJobSeekerResume(int jobSeekerID)
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
        [HttpDelete("DeleteJobListingById")]
        public async Task<IActionResult> DeleteJobListingById(int jobID)
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
        [HttpGet("JobSeekerContactDetails")]
        public async Task<IActionResult> GetJobSeekerContactDetails(int jobSeekerID)
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

    }
}
