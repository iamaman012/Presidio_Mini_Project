using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication( int jobSeekerID,int jobID)
        {

            try
            {
                var response = await _applicationService.SubmitApplication(jobID, jobSeekerID);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch(JobSeekerNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (ApplicationAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

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
                return NotFound("No job listings found.");
            }
        }
        [HttpPost("GetJobsFilterByLocation")]
        public async Task<IActionResult> GetJobsFilterByLocation( string location)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByLocationAsync(location);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound("No job listings found.");
            }
        }

        [HttpPost("GetJobsFilterBySalary")]
        public async Task<IActionResult> GetJobsFilterBySalary(double sRange,double eRange)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsBySalaryAsync(sRange,eRange);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost("GetJobsFilterByJobTitle")]
        public async Task<IActionResult> GetJobsFilterByTitle(string jobTitle)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByTitleAsync(jobTitle);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost("GetJobsFilterByJobType")]
        public async Task<IActionResult> GetJobsFilterByJobType(string jobType)
        {
            try
            {
                var response = await _jobListingService.GetJobListingsByJobTypeAsync(jobType);
                return Ok(response);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(InvalidJobTypeException e)
            {
                return NotFound($"Invali Job Type {jobType}");
            }
        }
        [HttpPost("GetApplicationStatus")]
        public async Task<IActionResult> GetApplicationStatus(int applicationId)
        {
            try
            {
                var response = await _applicationService.GetApplicationStatus(applicationId);
                return Ok(response);
            }
            catch (ApplicationNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost("GetApplicationFilteredByJobSeekerID")]
        public async Task<IActionResult> GetApplicationFilteredByJobSeekerID(int jobSeekerID)
        {
            try
            {
                var response = await _applicationService.GetApplicationByJobSeekerID(jobSeekerID);
                return Ok(response);
            }
            catch (ApplicationNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(JobSeekerNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
