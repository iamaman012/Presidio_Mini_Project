using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    public class JobSeekerRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, JobSeeker> jobSeekerRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            jobSeekerRepository = new JobSeekerRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddJobSeeker Tests
        [Test]
        public async Task AddJobSeeker_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            // Act
            var result = await jobSeekerRepository.Add(jobSeeker);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSeeker.UserID, result.UserID);
        }

        [Test]
        public async  Task AddJobSeeker_Fail()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()

            };

            await jobSeekerRepository.Add(jobSeeker);
            // Act & Assert
            // Adding the same job seeker should fail if there's a constraint violation
            Assert.ThrowsAsync<JobSeeKerAlreadyExistExceptiom>(async () => await jobSeekerRepository.Add(jobSeeker));
        }

       

        // UpdateJobSeeker Tests
        [Test]
        public async Task UpdateJobSeeker_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            var addedJobSeeker = await jobSeekerRepository.Add(jobSeeker);
            addedJobSeeker.UserID = 2;

            // Act
            var result = await jobSeekerRepository.Update(addedJobSeeker);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.UserID);
        }

        [Test]
        public void UpdateJobSeeker_Fail()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                JobSeekerID = 999,
                UserID = 999
            };

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await jobSeekerRepository.Update(jobSeeker));
        }


        // DeleteJobSeeker Tests
        [Test]
        public async Task DeleteJobSeeker_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            var addedJobSeeker = await jobSeekerRepository.Add(jobSeeker);

            // Act
            var result = await jobSeekerRepository.DeleteById(addedJobSeeker.JobSeekerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSeeker.UserID, result.UserID);
        }

        [Test]
        public void DeleteJobSeeker_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await jobSeekerRepository.DeleteById(999));
        }

       

        // GetJobSeekerById Tests
        [Test]
        public async Task GetJobSeekerById_Pass()
        {
            // Arrange
            var jobSeeker = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            var addedJobSeeker = await jobSeekerRepository.Add(jobSeeker);

            // Act
            var result = await jobSeekerRepository.GetById(addedJobSeeker.JobSeekerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addedJobSeeker.UserID, result.UserID);
        }

        [Test]
        public void GetJobSeekerById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await jobSeekerRepository.GetById(999));
        }

       

        // GetAllJobSeekers Tests
        [Test]
        public async Task GetAllJobSeekers_Pass()
        {
            // Arrange
            var jobSeeker1 = new JobSeeker
            {
                UserID = 1,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            var jobSeeker2 = new JobSeeker
            {
                UserID = 2,
                JobSeekerSkills = new List<JobSeekerSkill>(),
                JobSeekerEducations = new List<JobSeekerEducation>(),
                JobSeekerExperiences = new List<JobSeekerExperience>()
            };

            await jobSeekerRepository.Add(jobSeeker1);
            await jobSeekerRepository.Add(jobSeeker2);

            // Act
            var result = await jobSeekerRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSeekers_Fail()
        {
            // Arrange
            var jobSeeker1 = new JobSeeker
            {
                UserID = 1
            };

            var jobSeeker2 = new JobSeeker
            {
                UserID = 2
            };

            await jobSeekerRepository.Add(jobSeeker1);
            await jobSeekerRepository.Add(jobSeeker2);

            // Act
            var result = await jobSeekerRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSeekers_Exception()
        {
            // Act & Assert
            // Simulating an exception scenario, such as database connection failure
            context.Database.EnsureDeleted();
            Assert.ThrowsAsync<InvalidOperationException>(async () => await jobSeekerRepository.GetAll());
        }
    }
}
