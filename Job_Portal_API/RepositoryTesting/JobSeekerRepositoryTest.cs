using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    public class JobSeekerRepositoryTest
    {
        private JobPortalApiContext _context;
        private IRepository<int, JobSeeker> _jobSeekerRepository;

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB");
            _context = new JobPortalApiContext(optionsBuilder.Options);
            _jobSeekerRepository = new JobSeekerRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // Test for adding a JobSeeker (pass case)
        [Test]
        public async Task AddJobSeekerTest_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };

            // Act
            var result = await _jobSeekerRepository.Add(jobSeeker);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(jobSeeker.JobSeekerID, result.JobSeekerID);
        }

        // Test for adding a JobSeeker with existing UserID (exception case)
        [Test]
        public async Task AddJobSeekerTest_Exception_UserAlreadyExists()
        {
            // Arrange
            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            await _jobSeekerRepository.Add(jobSeeker);
            var duplicateJobSeeker = new JobSeeker { JobSeekerID = 2, UserID = 1 };

            // Act & Assert
            var ex = Assert.ThrowsAsync<JobSeeKerAlreadyExistExceptiom>(async () => await _jobSeekerRepository.Add(duplicateJobSeeker));
            Assert.AreEqual("Job Seeker Already Exists", ex.Message);
        }

        // Test for updating a JobSeeker (pass case)
        [Test]
        public async Task UpdateJobSeekerTest_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            await _jobSeekerRepository.Add(jobSeeker);
            jobSeeker.UserID = 2;

            // Act
            var result = await _jobSeekerRepository.Update(jobSeeker);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(jobSeeker.UserID, result.UserID);
        }

        // Test for updating a JobSeeker that doesn't exist (exception case)
        [Test]
        public void UpdateJobSeekerTest_Exception_NotFound()
        {
            // Arrange
            var jobSeeker = new JobSeeker { JobSeekerID = 99, UserID = 1 };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _jobSeekerRepository.Update(jobSeeker));
            Assert.AreEqual("Job Seeker Not Found", ex.Message);
        }

        // Test for deleting a JobSeeker by ID (pass case)
        [Test]
        public async Task DeleteJobSeekerByIdTest_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            await _jobSeekerRepository.Add(jobSeeker);

            // Act
            var result = await _jobSeekerRepository.DeleteById(jobSeeker.JobSeekerID);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(jobSeeker.JobSeekerID, result.JobSeekerID);
        }

        // Test for deleting a JobSeeker by non-existent ID (exception case)
        [Test]
        public void DeleteJobSeekerByIdTest_Exception_NotFound()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _jobSeekerRepository.DeleteById(99));
            Assert.AreEqual("Job Seeker Not Found", ex.Message);
        }

        // Test for getting a JobSeeker by ID (pass case)
        [Test]
        public async Task GetJobSeekerByIdTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            await _jobSeekerRepository.Add(jobSeeker);
            await _context.SaveChangesAsync();

            var skill1 = new JobSeekerSkill { JobSeekerSkillID = 1, JobSeekerID = 1, SkillName = "Skill A" };
            await _context.JobSeekerSkills.AddAsync(skill1);
            await _context.SaveChangesAsync();

            var education1 = new JobSeekerEducation { EducationID = 1, JobSeekerID = 1, Degree = "Bachelor's", Institution = "University A", Description = "dsdd", Location = "dwsd" };
            await _context.Educations.AddAsync(education1);
            await _context.SaveChangesAsync();

            var experience1 = new JobSeekerExperience { ExperienceID = 1, JobSeekerID = 1, JobTitle = "Developer", Description = "dsdd", Location = "dwsd", CompanyName = "ABC" };
            await _context.Experiences.AddAsync(experience1);
            await _context.SaveChangesAsync();

           

            // Act
            var result = await _jobSeekerRepository.GetById(jobSeeker.JobSeekerID);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(jobSeeker.JobSeekerID, result.JobSeekerID);
        }

        // Test for getting a JobSeeker by non-existent ID (exception case)
        [Test]
        public void GetJobSeekerByIdTest_Exception_NotFound()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerRepository.GetById(99));
        }
        [Test]
        public async Task GetAllJobSeekersTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            var user2 = new User
            {
                UserID = 2,
                Email = "abcd@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddRangeAsync(user1, user2);
            await _context.SaveChangesAsync();

            var skill1 = new JobSeekerSkill { JobSeekerSkillID = 1, JobSeekerID = 1, SkillName = "Skill A" };
            var skill2 = new JobSeekerSkill { JobSeekerSkillID = 2, JobSeekerID = 2, SkillName = "Skill B" };
            await _context.JobSeekerSkills.AddRangeAsync(skill1, skill2);
            await _context.SaveChangesAsync();

            var education1 = new JobSeekerEducation { EducationID = 1, JobSeekerID = 1, Degree = "Bachelor's", Institution = "University A",Description="dsdd",Location="dwsd" };
            var education2 = new JobSeekerEducation { EducationID = 2, JobSeekerID = 2, Degree = "Master's", Institution = "University B", Description = "dsdd", Location = "dwsd" };
            await _context.Educations.AddRangeAsync(education1, education2);
            await _context.SaveChangesAsync();

            var experience1 = new JobSeekerExperience { ExperienceID = 1, JobSeekerID = 1, JobTitle = "Developer", Description = "dsdd", Location = "dwsd",CompanyName="ABC" };
            var experience2 = new JobSeekerExperience { ExperienceID = 2, JobSeekerID = 2, JobTitle = "Engineer", Description = "dsdd", Location = "dwsd",CompanyName="ABC" };
            await _context.Experiences.AddRangeAsync(experience1, experience2);
            await _context.SaveChangesAsync();

            var jobSeeker1 = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            var jobSeeker2 = new JobSeeker { JobSeekerID = 2, UserID = 2 };
            await _context.JobSeekers.AddRangeAsync(jobSeeker1, jobSeeker2);
            await _context.SaveChangesAsync();

            

            // Act
            var result = await _jobSeekerRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Console.WriteLine($"Number of job seekers retrieved by GetAll: {result.Count()}");
            Assert.AreEqual(2, result.Count());

           
        }






    }
}
