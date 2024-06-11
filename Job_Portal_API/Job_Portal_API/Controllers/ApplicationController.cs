using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplication _applicationService;
        private readonly IJobListing _jobListingService;
        public ApplicationController(IApplication applicationService, IJobListing jobListingService)
        {
            _applicationService = applicationService;
            _jobListingService = jobListingService;
        }
        [Authorize(Roles = "JobSeeker")]
        [HttpPost("ApplyForJob")]
        public async Task<IActionResult> SubmitApplication([Required] int jobSeekerID,[Required]int jobID)
        {

            try
            {
                var response = await _applicationService.SubmitApplication(jobID, jobSeekerID);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404,e.Message));
            }
            catch(JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (ApplicationAlreadyExistException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                var response = await _jobListingService.GetAllJobListingsAsync();
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
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetJobsFilterByLocation")]
        public async Task<IActionResult> GetJobsFilterByLocation([Required] string location)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByLocationAsync(location);
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
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetJobsFilterBySalary")]
        public async Task<IActionResult> GetJobsFilterBySalary([Required] double sRange,[Required] double eRange)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsBySalaryAsync(sRange,eRange);
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
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetJobsFilterByJobTitle")]
        public async Task<IActionResult> GetJobsFilterByTitle([Required] string jobTitle)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByTitleAsync(jobTitle);
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
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetJobsFilterByJobType")]
        public async Task<IActionResult> GetJobsFilterByJobType([Required] string jobType)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByJobTypeAsync(jobType);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch(InvalidJobTypeException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetApplicationStatus")]
        public async Task<IActionResult> GetApplicationStatus(int applicationId)
        {
            try
            {
                var response = await _applicationService.GetApplicationStatus(applicationId);
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
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetApplicationFilteredByJobSeekerID")]
        public async Task<IActionResult> GetApplicationFilteredByJobSeekerID([Required] int jobSeekerID)
        {
            try
            {
                var response = await _applicationService.GetApplicationByJobSeekerID(jobSeekerID);
                return Ok(response);
            }
            catch (ApplicationNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch(JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "JobSeeker")]
        [HttpDelete("DeleteApplicationById")]
        public async Task<IActionResult> DeleteApplicationById([Required] int applicationId)
        {
            try
            {
                var response = await _applicationService.DeleteApplicationById(applicationId);
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
    }
}
