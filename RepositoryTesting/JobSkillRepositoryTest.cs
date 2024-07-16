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
    public class JobSkillRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, JobSkill> jobSkillRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            jobSkillRepository = new JobSkillRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddJobSkill Tests
        [Test]
        public async Task AddJobSkill_Pass()
        {
            // Arrange
            var jobSkill = new JobSkill
            {
                JobID = 1,
                SkillName = "C#"
            };

            // Act
            var result = await jobSkillRepository.Add(jobSkill);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSkill.JobID, result.JobID);
            Assert.AreEqual(jobSkill.SkillName, result.SkillName);
        }

        [Test]
        public async Task AddJobSkill_Fail()
        {
            // Arrange
            var jobSkill = new JobSkill
            {   JobSkillID=1,
                JobID = 1,
                SkillName = "C#"
            };

            await jobSkillRepository.Add(jobSkill);

            // Act & Assert
            // Adding the same job skill should fail due to a constraint violation
            Assert.ThrowsAsync<ArgumentException>(async () => await jobSkillRepository.Add(jobSkill));
        }

        [Test]
        public void AddJobSkill_Exception()
        {
            // Arrange
            var jobSkill = new JobSkill(); // Invalid job skill object

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await jobSkillRepository.Add(jobSkill));
        }

        // UpdateJobSkill Tests
        [Test]
        public async Task UpdateJobSkill_Pass()
        {
            // Arrange
            var jobSkill = new JobSkill
            {
                JobID = 1,
                SkillName = "C#"
            };

            var addedJobSkill = await jobSkillRepository.Add(jobSkill);
            addedJobSkill.SkillName = "Java";

            // Act
            var result = await jobSkillRepository.Update(addedJobSkill);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Java", result.SkillName);
        }

        [Test]
        public async Task UpdateJobSkill_Fail()
        {
            // Arrange
            var jobSkill = new JobSkill
            {
                JobSkillID = 999,
                JobID = 999,
                SkillName = "NonExistentSkill"
            };

            // Act & Assert
            Assert.ThrowsAsync<JobSkillNotFoundException>(async () => await jobSkillRepository.Update(jobSkill));
        }

       

        // DeleteJobSkill Tests
        [Test]
        public async Task DeleteJobSkill_Pass()
        {
            // Arrange
            var jobSkill = new JobSkill
            {
                JobID = 1,
                SkillName = "C#"
            };

            var addedJobSkill = await jobSkillRepository.Add(jobSkill);

            // Act
            var result = await jobSkillRepository.DeleteById(addedJobSkill.JobSkillID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSkill.JobID, result.JobID);
        }

        [Test]
        public async Task DeleteJobSkill_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobSkillNotFoundException>(async () => await jobSkillRepository.DeleteById(999));
        }

        

        // GetJobSkillById Tests
        [Test]
        public async Task GetJobSkillById_Pass()
        {
            // Arrange
            var jobSkill = new JobSkill
            {
                JobID = 1,
                SkillName = "C#"
            };

            var addedJobSkill = await jobSkillRepository.Add(jobSkill);

            // Act
            var result = await jobSkillRepository.GetById(addedJobSkill.JobSkillID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSkill.JobID, result.JobID);
        }

        [Test]
        public async Task GetJobSkillById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobSkillNotFoundException>(async () => await jobSkillRepository.GetById(999));
        }

        

        // GetAllJobSkills Tests
        [Test]
        public async Task GetAllJobSkills_Pass()
        {
            // Arrange
            var jobSkill1 = new JobSkill
            {
                JobID = 1,
                SkillName = "C#"
            };

            var jobSkill2 = new JobSkill
            {
                JobID = 2,
                SkillName = "Java"
            };

            await jobSkillRepository.Add(jobSkill1);
            await jobSkillRepository.Add(jobSkill2);

            // Act
            var result = await jobSkillRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSkills_Fail()
        {
            // Act
            var result = await jobSkillRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

    }
}
