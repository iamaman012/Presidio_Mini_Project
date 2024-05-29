using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeeker _jobSeekerService;

        public JobSeekerController(IJobSeeker jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        // Endpoint to add a new job seeker experience
        [HttpPost("experience")]
        public async Task<IActionResult> AddExperience([FromBody] ExperienceDTO experienceDTO)
        {
            if (experienceDTO == null)
            {
                return BadRequest("Experience data is required.");
            }

            try
            {
                var result = await _jobSeekerService.AddExperience(experienceDTO);
                return Ok(result);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound("Job seeker not found.");
            }
        }

        // Endpoint to add a new job seeker education
        [HttpPost("education")]
        public async Task<IActionResult> AddEducation([FromBody] EducationDTO educationDTO)
        {
            if (educationDTO == null)
            {
                return BadRequest("Education data is required.");
            }

            try
            {
                var result = await _jobSeekerService.AddEducation(educationDTO);
                return Ok(result);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound("Job seeker not found.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobSeekerById(int id)
        {
            try
            {
                var result = await _jobSeekerService.GetResumeByJobSeekerId(id);
                return Ok(result);
            }
            catch (JobSeekerNotFoundException)
            {
                return NotFound(new { message = "Job Seeker not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddJobSeekerSkills([FromBody] JobSeekerSkillDTO jobSeekerSkillDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var addedSkills = await _jobSeekerService.AddJobSeekerSkillsAsync(jobSeekerSkillDto);
                return Ok(addedSkills);

            }
            catch(JobSeekerNotFoundException e)
            {
                return NotFound("Job Seeker not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           

            
        }
    }
}
