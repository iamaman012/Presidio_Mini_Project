using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    public class ExperienceRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, JobSeekerExperience> experienceRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            experienceRepository = new ExperienceRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddJobSeekerExperience Tests
        [Test]
        public async Task AddJobSeekerExperience_Pass()
        {
            // Arrange
            var experience = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            // Act
            var result = await experienceRepository.Add(experience);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(experience.JobSeekerID, result.JobSeekerID);
            Assert.AreEqual(experience.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task AddJobSeekerExperience_Fail()
        {
            // Arrange
            var experience = new JobSeekerExperience
            { ExperienceID=1,
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            await experienceRepository.Add(experience);

            // Act & Assert
            // Adding the same experience should fail due to a constraint violation
            Assert.ThrowsAsync<ArgumentException>(async () => await experienceRepository.Add(experience));
        }

       

        // UpdateJobSeekerExperience Tests
        [Test]
        public async Task UpdateJobSeekerExperience_Pass()
        {
            // Arrange
            var experience = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            var addedExperience = await experienceRepository.Add(experience);
            addedExperience.JobTitle = "Senior Software Engineer";

            // Act
            var result = await experienceRepository.Update(addedExperience);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Senior Software Engineer", result.JobTitle);
        }

        [Test]
        public async Task UpdateJobSeekerExperience_Fail()
        {
            // Arrange
            var experience = new JobSeekerExperience
            {
                ExperienceID = 999,
                JobSeekerID = 999,
                JobTitle = "NonExistentTitle"
            };

            // Act & Assert
            Assert.ThrowsAsync<ExperienceNotFoundException>(async () => await experienceRepository.Update(experience));
        }

       

        // DeleteJobSeekerExperience Tests
        [Test]
        public async Task DeleteJobSeekerExperience_Pass()
        {
            // Arrange
            var experience = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            var addedExperience = await experienceRepository.Add(experience);

            // Act
            var result = await experienceRepository.DeleteById(addedExperience.ExperienceID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(experience.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task DeleteJobSeekerExperience_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<ExperienceNotFoundException>(async () => await experienceRepository.DeleteById(999));
        }

      

        // GetJobSeekerExperienceById Tests
        [Test]
        public async Task GetJobSeekerExperienceById_Pass()
        {
            // Arrange
            var experience = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            var addedExperience = await experienceRepository.Add(experience);

            // Act
            var result = await experienceRepository.GetById(addedExperience.ExperienceID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(experience.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task GetJobSeekerExperienceById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<ExperienceNotFoundException>(async () => await experienceRepository.GetById(999));
        }

       

        // GetAllJobSeekerExperiences Tests
        [Test]
        public async Task GetAllJobSeekerExperiences_Pass()
        {
            // Arrange
            var experience1 = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Company",
                Location = "City",
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Description = "Worked on various projects"
            };

            var experience2 = new JobSeekerExperience
            {
                JobSeekerID = 2,
                JobTitle = "Senior Software Engineer",
                CompanyName = "Another Tech Company",
                Location = "Town",
                StartDate = new DateTime(2019, 1, 1),
                EndDate = new DateTime(2021, 12, 31),
                Description = "Led various projects"
            };

            await experienceRepository.Add(experience1);
            await experienceRepository.Add(experience2);

            // Act
            var result = await experienceRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSeekerExperiences_Fail()
        {
            // Act
            var result = await experienceRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

    }
}
