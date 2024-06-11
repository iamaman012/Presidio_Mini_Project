using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Job_Portal_API.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesTest
{
    public class JobListingServiceTest
    {
        private JobPortalApiContext _context;
        private IRepository<int, JobListing> _jobListingRepository;
        private IRepository<int, Employer> _employerRepository;
        private IRepository<int, Application> _applicationRepository;
        private IRepository<int, JobSeeker> _jobSeekerRepository;
        private IRepository<int, User> _userRepository;
        private IJobListing _jobListingService;
        private IRepository<int,JobSkill> _jobSkillRepository;

        [SetUp]
        public async Task Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB");
            _context = new JobPortalApiContext(optionsBuilder.Options);
            _jobListingRepository = new JobListingRepository(_context);
            _employerRepository = new EmployerRepository(_context);
            _applicationRepository = new ApplicationRepository(_context);
            _jobSeekerRepository = new JobSeekerRepository(_context);
            _userRepository = new UserRepository(_context);
            _jobSkillRepository = new JobSkillRepository(_context);
            _jobListingService = new JobListingService(
                _jobListingRepository,
                _employerRepository,
                _applicationRepository,
                _userRepository,
                _jobSeekerRepository,
                _jobSkillRepository
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddJobListingAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company",CompanyDescription="Software",CompanyLocation="Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();

            var jobListingDto = new JobListingDTO
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = "FullTime",
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                Skills = new List<JobSkillDTO>
                {
                    new JobSkillDTO { SkillName = "C#" },
                    new JobSkillDTO { SkillName = "ASP.NET" }
                }
            };

            // Act
            var result = await _jobListingService.AddJobListingAsync(jobListingDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobListingDto.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task AddJobListingAsync_ShouldFail_InvalidJobType()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListingDto = new JobListingDTO
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = "InvalidJobType",
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                Skills = new List<JobSkillDTO>
                {
                    new JobSkillDTO { SkillName = "C#" },
                    new JobSkillDTO { SkillName = "ASP.NET" }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _jobListingService.AddJobListingAsync(jobListingDto));
            Assert.AreEqual("Invalid Job type", ex.Message);
        }

        [Test]
        public async Task AddJobListingAsync_ShouldThrow_UserNotFoundException()
        {
            // Arrange
            var jobListingDto = new JobListingDTO
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = "FullTime",
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = 99, // Non-existent EmployerID
                Skills = new List<JobSkillDTO>
                {
                    new JobSkillDTO { SkillName = "C#" },
                    new JobSkillDTO { SkillName = "ASP.NET" }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _jobListingService.AddJobListingAsync(jobListingDto));
            Assert.AreEqual("Employer Not Exist", ex.Message);
        }

        [Test]
        public async Task GetAllJobListingsAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.GetAllJobListingsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllJobListingsAsync_ShouldFail_NoJobListings()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NoJobExistException>(async () => await _jobListingService.GetAllJobListingsAsync());
            Assert.AreEqual("No Job Listings Exist", ex.Message);
        }

        [Test]
        public async Task GetJobListingByIdAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            var addedJobListing = await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.GetJobListingByIdAsync(addedJobListing.JobID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addedJobListing.JobID, result.JobID);
        }

        [Test]
        public async Task GetJobListingByIdAsync_ShouldFail_InvalidId()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<JobListingNotFoundException>(async () => await _jobListingService.GetJobListingByIdAsync(999));
            Assert.AreEqual("Job Listing Not Found", ex.Message);
        }

        [Test]
        public async Task GetJobListingsByEmployerIdAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.GetJobListingsByEmployerIdAsync(employer.EmployerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetJobListingsByEmployerIdAsync_ShouldFail_NoJobListings()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _jobListingService.GetJobListingsByEmployerIdAsync(999));
            Assert.AreEqual("Employer Not Exist", ex.Message);
        }

        [Test]
        public async Task GetJobListingsByLocationAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.GetJobListingsByLocationAsync("Remote");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetJobListingsByLocationAsync_ShouldFail_NoJobListings()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NoJobExistException>(async () => await _jobListingService.GetJobListingsByLocationAsync("Mars"));
            Assert.AreEqual("No Job Listings Exist", ex.Message);
        }

        [Test]
        public async Task GetJobListingsByTypeAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.GetJobListingsByJobTypeAsync(JobType.FullTime.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetJobListingsByTypeAsync_ShouldFail_NoJobListings()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NoJobExistException>(async () => await _jobListingService.GetJobListingsByJobTypeAsync(JobType.PartTime.ToString()));
            Assert.AreEqual("No Job Listings Exist", ex.Message);
        }

        //[Test]
        //public async Task UpdateJobListingAsync_ShouldPass()
        //{
        //    // Arrange
        //    var employer = new Employer { UserID = 1, CompanyName = "Test Company" };
        //    await _employerRepository.Add(employer);

        //    var jobListing = new JobListing
        //    {
        //        JobTitle = "Software Engineer",
        //        JobDescription = "Develop and maintain software applications.",
        //        JobType = JobType.FullTime,
        //        Location = "Remote",
        //        Salary = 100000,
        //        PostingDate = DateTime.UtcNow,
        //        ClosingDate = DateTime.UtcNow.AddMonths(1),
        //        EmployerID = employer.UserID,
        //        JobSkills = new List<JobSkill>
        //        {
        //            new JobSkill { SkillName = "C#" },
        //            new JobSkill { SkillName = "ASP.NET" }
        //        }
        //    };
        //    var addedJobListing = await _jobListingRepository.Add(jobListing);

        //    var updateJobListingDto = new JobListingDTO
        //    {
        //        JobTitle = "Senior Software Engineer",
        //        JobDescription = "Develop and maintain software applications with advanced responsibilities.",
        //        JobType = "FullTime",
        //        Location = "Remote",
        //        Salary = 120000,
        //        PostingDate = DateTime.UtcNow,
        //        ClosingDate = DateTime.UtcNow.AddMonths(1),
        //        EmployerID = employer.UserID,
        //        Skills = new List<JobSkillDTO>
        //        {
        //            new JobSkillDTO { SkillName = "C#" },
        //            new JobSkillDTO { SkillName = "ASP.NET" },
        //            new JobSkillDTO { SkillName = "Azure" }
        //        }
        //    };

        //    // Act
        //    var result = await _jobListingService.UpdateJobListingAsync(addedJobListing.JobID, updateJobListingDto);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(updateJobListingDto.JobTitle, result.JobTitle);
        //}

        //[Test]
        //public async Task UpdateJobListingAsync_ShouldFail_InvalidJobType()
        //{
        //    // Arrange
        //    var employer = new Employer { UserID = 1, CompanyName = "Test Company" };
        //    await _employerRepository.Add(employer);

        //    var jobListing = new JobListing
        //    {
        //        JobTitle = "Software Engineer",
        //        JobDescription = "Develop and maintain software applications.",
        //        JobType = JobType.FullTime,
        //        Location = "Remote",
        //        Salary = 100000,
        //        PostingDate = DateTime.UtcNow,
        //        ClosingDate = DateTime.UtcNow.AddMonths(1),
        //        EmployerID = employer.UserID,
        //        JobSkills = new List<JobSkill>
        //        {
        //            new JobSkill { SkillName = "C#" },
        //            new JobSkill { SkillName = "ASP.NET" }
        //        }
        //    };
        //    var addedJobListing = await _jobListingRepository.Add(jobListing);

        //    var updateJobListingDto = new JobListingDTO
        //    {
        //        JobTitle = "Senior Software Engineer",
        //        JobDescription = "Develop and maintain software applications with advanced responsibilities.",
        //        JobType = "InvalidJobType",
        //        Location = "Remote",
        //        Salary = 120000,
        //        PostingDate = DateTime.UtcNow,
        //        ClosingDate = DateTime.UtcNow.AddMonths(1),
        //        EmployerID = employer.UserID,
        //        Skills = new List<JobSkillDTO>
        //        {
        //            new JobSkillDTO { SkillName = "C#" },
        //            new JobSkillDTO { SkillName = "ASP.NET" },
        //            new JobSkillDTO { SkillName = "Azure" }
        //        }
        //    };

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _jobListingService.UpdateJobListingAsync(addedJobListing.JobID, updateJobListingDto));
        //    Assert.AreEqual("Invalid Job type", ex.Message);
        //}

        //[Test]
        //public async Task UpdateJobListingAsync_ShouldThrow_JobListingNotFoundException()
        //{
        //    // Arrange
        //    var updateJobListingDto = new JobListingDTO
        //    {
        //        JobTitle = "Senior Software Engineer",
        //        JobDescription = "Develop and maintain software applications with advanced responsibilities.",
        //        JobType = "FullTime",
        //        Location = "Remote",
        //        Salary = 120000,
        //        PostingDate = DateTime.UtcNow,
        //        ClosingDate = DateTime.UtcNow.AddMonths(1),
        //        EmployerID = 1,
        //        Skills = new List<JobSkillDTO>
        //        {
        //            new JobSkillDTO { SkillName = "C#" },
        //            new JobSkillDTO { SkillName = "ASP.NET" },
        //            new JobSkillDTO { SkillName = "Azure" }
        //        }
        //    };

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<JobListingNotFoundException>(async () => await _jobListingService.UpdateJobListingAsync(999, updateJobListingDto));
        //    Assert.AreEqual("Job listing not found", ex.Message);
        //}

        [Test]
        public async Task DeleteJobListingAsync_ShouldPass()
        {
            // Arrange
            var employer = new Employer { UserID = 1, CompanyName = "Test Company", CompanyDescription = "Software", CompanyLocation = "Banglore" };
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();


            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Develop and maintain software applications.",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 100000,
                PostingDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow.AddMonths(1),
                EmployerID = employer.EmployerID,
                JobSkills = new List<JobSkill>
                {
                    new JobSkill { SkillName = "C#" },
                    new JobSkill { SkillName = "ASP.NET" }
                }
            };
            var addedJobListing = await _jobListingRepository.Add(jobListing);

            // Act
            var result = await _jobListingService.DeleteJobListingById(addedJobListing.JobID);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task DeleteJobListingAsync_ShouldFail_JobListingNotFoundException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<JobListingNotFoundException>(async () => await _jobListingService.DeleteJobListingById(999));
            Assert.AreEqual("Job Listing Not Found", ex.Message);
        }

    }
}
