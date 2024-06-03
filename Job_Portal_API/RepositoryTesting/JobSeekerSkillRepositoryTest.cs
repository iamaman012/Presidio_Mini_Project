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
    public class JobSeekerSkillRepositoryTest
    {
        private JobPortalApiContext context;
        private IRangeRepository<int, JobSeekerSkill> jobSeekerSkillRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            jobSeekerSkillRepository = new JobSeekerSkillRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddJobSeekerSkill Tests
        [Test]
        public async Task AddJobSeekerSkill_Pass()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            // Act
            var result = await jobSeekerSkillRepository.Add(jobSeekerSkill);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSeekerSkill.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task AddJobSeekerSkill_Fail()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            await jobSeekerSkillRepository.Add(jobSeekerSkill);

            // Act & Assert
            // Adding the same job seeker skill should fail due to a constraint violation
            Assert.ThrowsAsync<ArgumentException>(async () => await jobSeekerSkillRepository.Add(jobSeekerSkill));
        }



        // UpdateJobSeekerSkill Tests
        [Test]
        public async Task UpdateJobSeekerSkill_Pass()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            var addedJobSeekerSkill = await jobSeekerSkillRepository.Add(jobSeekerSkill);
            addedJobSeekerSkill.JobSeekerID = 2;

            // Act
            var result = await jobSeekerSkillRepository.Update(addedJobSeekerSkill);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.JobSeekerID);
        }

        [Test]
        public async Task UpdateJobSeekerSkill_Fail()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerSkillID = 999,
                JobSeekerID = 999,
                SkillName = "Skill1"
            };

            // Act & Assert
            Assert.ThrowsAsync<JobSeekerSkillNotFoundException>(async () => await jobSeekerSkillRepository.Update(jobSeekerSkill));
        }



        // DeleteJobSeekerSkill Tests
        [Test]
        public async Task DeleteJobSeekerSkill_Pass()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            var addedJobSeekerSkill = await jobSeekerSkillRepository.Add(jobSeekerSkill);

            // Act
            var result = await jobSeekerSkillRepository.DeleteById(addedJobSeekerSkill.JobSeekerSkillID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSeekerSkill.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task DeleteJobSeekerSkill_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobSeekerSkillNotFoundException>(async () => await jobSeekerSkillRepository.DeleteById(999));
        }



        // GetJobSeekerSkillById Tests
        [Test]
        public async Task GetJobSeekerSkillById_Pass()
        {
            // Arrange
            var jobSeekerSkill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            var addedJobSeekerSkill = await jobSeekerSkillRepository.Add(jobSeekerSkill);

            // Act
            var result = await jobSeekerSkillRepository.GetById(addedJobSeekerSkill.JobSeekerSkillID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jobSeekerSkill.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task GetJobSeekerSkillById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<JobSeekerSkillNotFoundException>(async () => await jobSeekerSkillRepository.GetById(999));
        }



        // GetAllJobSeekerSkills Tests
        [Test]
        public async Task GetAllJobSeekerSkills_Pass()
        {
            // Arrange
            var jobSeekerSkill1 = new JobSeekerSkill
            {
                JobSeekerID = 1,
                JobSeekerSkillID = 1,
                SkillName = "Skill1"
            };

            var jobSeekerSkill2 = new JobSeekerSkill
            {
                JobSeekerID = 2,
                JobSeekerSkillID = 2,
                SkillName = "Skill2"
            };

            await jobSeekerSkillRepository.Add(jobSeekerSkill1);
            await jobSeekerSkillRepository.Add(jobSeekerSkill2);

            // Act
            var result = await jobSeekerSkillRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSeekerSkills_Fail()
        {
            // Act
            var result = await jobSeekerSkillRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }



        // AddRangeJobSeekerSkills Tests
        [Test]
        public async Task AddRangeJobSeekerSkills_Pass()
        {
            // Arrange
            var jobSeekerSkills = new List<JobSeekerSkill>
            {
                new JobSeekerSkill {  JobSeekerID = 1,JobSeekerSkillID = 1, SkillName = "Skill1" },
                new JobSeekerSkill {  JobSeekerID = 2,JobSeekerSkillID = 2,SkillName = "Skill2" }
            };

            // Act
            var result = await jobSeekerSkillRepository.AddRange(jobSeekerSkills);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}

       

       
