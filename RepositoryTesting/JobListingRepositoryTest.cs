using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    public class JobListingRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, JobListing> jobListingRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            jobListingRepository = new JobListingRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // Add Tests
        [Test]
        public async Task AddJobListing_Pass()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Developing software applications",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 80000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 1
            };

            // Act
            var result = await jobListingRepository.Add(jobListing);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobListing.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task AddJobListing_Fail()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobID = 1, // Providing existing ID to force failure
                JobTitle = "Software Engineer"
            };

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await jobListingRepository.Add(jobListing));
        }

      

        // Update Tests
        [Test]
        public async Task UpdateJobListing_Pass()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Developing software applications",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 80000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 1
            };

            var addedJobListing = await jobListingRepository.Add(jobListing);
            addedJobListing.JobTitle = "Senior Software Engineer";

            // Act
            var result = await jobListingRepository.Update(addedJobListing);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Senior Software Engineer", result.JobTitle);
        }

        [Test]
        public async Task UpdateJobListing_Fail()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobID = 999, // Providing non-existing ID to force failure
                JobTitle = "NonExistentJobTitle"
            };

            // Act & Assert
            Assert.ThrowsAsync<JobListingNotFoundException>(async () => await jobListingRepository.Update(jobListing));
        }

       

        // Delete Tests
        [Test]
        public async Task DeleteJobListing_Pass()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Developing software applications",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 80000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 1
            };

            var addedJobListing = await jobListingRepository.Add(jobListing);

            // Act
            var result = await jobListingRepository.DeleteById(addedJobListing.JobID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobListing.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task DeleteJobListing_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobListingNotFoundException>(async () => await jobListingRepository.DeleteById(999));
        }

        // GetById Tests
        [Test]
        public async Task GetJobListingById_Pass()
        {
            // Arrange
            var jobListing = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Developing software applications",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 80000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 1
            };

            var addedJobListing = await jobListingRepository.Add(jobListing);

            // Act
            var result = await jobListingRepository.GetById(addedJobListing.JobID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobListing.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task GetJobListingById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobListingNotFoundException>(async () => await jobListingRepository.GetById(999));
        }

        // GetAll Tests
        [Test]
        public async Task GetAllJobListings_Pass()
        {
            // Arrange
            var jobListing1 = new JobListing
            {
                JobTitle = "Software Engineer",
                JobDescription = "Developing software applications",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 80000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 1
            };

            var jobListing2 = new JobListing
            {
                JobTitle = "Data Scientist",
                JobDescription = "Analyze and interpret complex datasets",
                JobType = JobType.FullTime,
                Location = "Remote",
                Salary = 90000,
                PostingDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(30),
                EmployerID = 2
            };

            await jobListingRepository.Add(jobListing1);
            await jobListingRepository.Add(jobListing2);

            // Act
            var result = await jobListingRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobListings_Fail()
        {
            // Act
            var result = await jobListingRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
