﻿using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _service;
        private readonly IUser _userService;
        private readonly IApplication _applicationService;
        private readonly IJobListing _jobListingService;
        public AdminController(IAdmin service, IUser userService, IApplication applicationService, IJobListing jobListingService)
        {
            _service = service;
            _userService = userService;
            _applicationService = applicationService;
            _jobListingService = jobListingService;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _service.GetAllUsers();
                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(404, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpGet("GetAllApplications")]
        public async Task<IActionResult> GetAllApplications()
        {
            try
            {
                var result = await _service.GetAllApplications();
                return Ok(result);
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
        [HttpGet("GetAllEmployers")]
        public async Task<IActionResult> GetAllEmployers()
        {
            try
            {
                var result = await _service.GetAllEmployers();
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

        [HttpGet("GetAllJobSeekers")]
        public async Task<IActionResult> GetAllJobSeekers()
        {
            try
            {
                var result = await _service.GetAllJobSeekers();
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

        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                var result = await _service.GetAllJobListings();
                return Ok(result);
            }
            catch (NoJobExistException e)
            {
                return NotFound(new ErrorModelDTO(404, e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = new ErrorModelDTO(500, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            try
            {
                var result = await _userService.DeleteUserById(userId);
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
        [HttpDelete("DeleteApplicationById")]
        public async Task<IActionResult> DeleteApplicationById(int applicationId)
        {
            try
            {
                var result = await _applicationService.DeleteApplicationById(applicationId);
                return Ok(result);
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
        [HttpDelete("DeleteJobListingById")]
        public async Task<IActionResult> DeleteJobListingById(int jobID)
        {
            try
            {
                var result = await _jobListingService.DeleteJobListingById(jobID);
                return Ok(result);
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
