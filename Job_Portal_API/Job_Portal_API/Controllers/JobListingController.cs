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

        public JobListingController(IJobListing service)
        {
            _service = service;
        }
        [Authorize(Roles = "Employer,Admin")]
        [HttpPost("AddJobListing")]
        public async Task<IActionResult> AddJobListing([FromBody] JobListingDTO jobListingDto)
        {
            if (jobListingDto == null)
            {
                return BadRequest("Invalid job listing data.");
            }

            try
            {
                var result = await _service.AddJobListingAsync(jobListingDto);
                return Ok(result);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            }
            catch (Exception e)
            {
                return NotFound("Employer not found.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobListings()
        {
            try
            {
                var jobListings = await _service.GetAllJobListingsAsync();
                return Ok(jobListings);
            }
            catch (JobListingNotFoundException e)
            {
                return NotFound("No job listings found.");
            }
        }
        [HttpGet("by-employer/{employerId}")]
        
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
            catch(JobListingNotFoundException e)
            {
                return NotFound("No job listings found.");
            }
        }

    }
}
