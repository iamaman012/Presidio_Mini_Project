using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeeker _jobSeekerService;
        private readonly IApplication _applicationService;

        public JobSeekerController(IJobSeeker jobSeekerService, IApplication applicationService)
        {
            _jobSeekerService = jobSeekerService;
            _applicationService = applicationService;
        }

        // Endpoint to add a new job seeker experience
        [Authorize(Roles ="JobSeeker")]
        [HttpPost("AddExperience")]
        public async Task<IActionResult> AddExperience( ExperienceDTO experienceDTO)
        {
            

            try
            {
                var result = await _jobSeekerService.AddExperience(experienceDTO);
                return Ok(result);
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

        // Endpoint to add a new job seeker education
        [Authorize(Roles = "JobSeeker")]
        [HttpPost("AddEducation")]
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
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("GetResumeById")]
        
        public async Task<IActionResult> GetResumeById([Required]int jobSeekerId)
        {
            try
            {
                var result = await _jobSeekerService.GetResumeByJobSeekerId(jobSeekerId);
                return Ok(result);
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
        [Authorize(Roles = "JobSeeker")]
        [HttpPost("AddSkills")]
        public async Task<IActionResult> AddJobSeekerSkills([FromBody] JobSeekerSkillDTO jobSeekerSkillDto)
        {
            try
            {   if(jobSeekerSkillDto.SkillNames.Count == 0)
                {
                    return BadRequest(new ErrorModelDTO(400, "Skill Name is Required"));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var addedSkills = await _jobSeekerService.AddJobSeekerSkillsAsync(jobSeekerSkillDto);
                return Ok(addedSkills);

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
        [HttpGet("GetApplicationsByJobSeekerID")]
        public async Task<IActionResult> GetApplicationFilteredByJobSeekerID( [Required]int jobSeekerID)
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
        [Authorize(Roles = "JobSeeker")]
        [HttpDelete("DeleteExperienceById")]
        public async Task<IActionResult> DeleteExperienceById([Required]int experienceId)
        {
            try
            {
                var result = await _jobSeekerService.DeleteExperirnceById(experienceId);
                return Ok(result);
            }
            catch (ExperienceNotFoundException e)
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
        [HttpDelete("DeleteEducationById")]
        public async Task<IActionResult> DeleteEducationById([Required] int educationId)
        {
            try
            {
                var result = await _jobSeekerService.DeleteEducationById(educationId);
                return Ok(result);
            }
            catch (EducationNotFoundException e)
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
        [HttpDelete("DeleteSkillById")]
        public async Task<IActionResult> DeleteSkillById([Required] int skillId)
        {
            try
            {
                var result = await _jobSeekerService.DeleteSkillById(skillId);
                return Ok(result);
            }
            catch (JobSeekerSkillNotFoundException e)
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
        [HttpPut("UpdateEducation")]
        public async Task<IActionResult> UpdateEducation(EducationResponseDTO educationDTO)
        {
            try
            {
                    var result = await _jobSeekerService.UpdateEducation(educationDTO);
                return Ok(result);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch(EducationNotFoundException e)
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
        [HttpPut("UpdateExperience")]
        public async Task<IActionResult> UpdateExperience(ExperienceResponseDTO experienceDTO)
        {
            try
            {
                var result = await _jobSeekerService.UpdateExperience(experienceDTO);
                return Ok(result);
            }
            catch (JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (ExperienceNotFoundException e)
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
        [HttpPut("UpdateSkill")]
        public async Task<IActionResult> UpdateSkill([Required] int jobSeekerId,[Required]int skillId,[Required]string skillName)
        {
            try
            {
                var result = await _jobSeekerService.UpdateSkill(jobSeekerId,skillId,skillName);
                return Ok(result);
            }
            catch(JobSeekerNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404,e.Message));
            }
            catch(JobSeekerSkillNotFoundException e)
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
