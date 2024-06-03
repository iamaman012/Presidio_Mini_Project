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
    public class EducationRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, JobSeekerEducation> educationRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            educationRepository = new EducationRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddJobSeekerEducation Tests
        [Test]
        public async Task AddJobSeekerEducation_Pass()
        {
            // Arrange
            var education = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            // Act
            var result = await educationRepository.Add(education);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(education.JobSeekerID, result.JobSeekerID);
            Assert.AreEqual(education.Degree, result.Degree);
        }

        [Test]
        public async Task AddJobSeekerEducation_Fail()
        {
            // Arrange
            var education = new JobSeekerEducation
            {   EducationID=1,
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            await educationRepository.Add(education);

            // Act & Assert
            // Adding the same education should fail due to a constraint violation
            Assert.ThrowsAsync<ArgumentException>(async () => await educationRepository.Add(education));
        }

       

        // UpdateJobSeekerEducation Tests
        [Test]
        public async Task UpdateJobSeekerEducation_Pass()
        {
            // Arrange
            var education = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            var addedEducation = await educationRepository.Add(education);
            addedEducation.Degree = "MSc Computer Science";

            // Act
            var result = await educationRepository.Update(addedEducation);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("MSc Computer Science", result.Degree);
        }

        [Test]
        public async Task UpdateJobSeekerEducation_Fail()
        {
            // Arrange
            var education = new JobSeekerEducation
            {
                EducationID = 999,
                JobSeekerID = 999,
                Degree = "NonExistentDegree"
            };

            // Act & Assert
            Assert.ThrowsAsync<EducationNotFoundException>(async () => await educationRepository.Update(education));
        }

       

        // DeleteJobSeekerEducation Tests
        [Test]
        public async Task DeleteJobSeekerEducation_Pass()
        {
            // Arrange
            var education = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            var addedEducation = await educationRepository.Add(education);

            // Act
            var result = await educationRepository.DeleteById(addedEducation.EducationID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(education.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task DeleteJobSeekerEducation_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<EducationNotFoundException>(async () => await educationRepository.DeleteById(999));
        }

        

        // GetJobSeekerEducationById Tests
        [Test]
        public async Task GetJobSeekerEducationById_Pass()
        {
            // Arrange
            var education = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            var addedEducation = await educationRepository.Add(education);

            // Act
            var result = await educationRepository.GetById(addedEducation.EducationID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(education.JobSeekerID, result.JobSeekerID);
        }

        [Test]
        public async Task GetJobSeekerEducationById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<EducationNotFoundException>(async () => await educationRepository.GetById(999));
        }

        

        // GetAllJobSeekerEducations Tests
        [Test]
        public async Task GetAllJobSeekerEducations_Pass()
        {
            // Arrange
            var education1 = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "BSc Computer Science",
                Institution = "XYZ University",
                Location = "City",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Bachelor's degree in computer science",
                GPA = 3.8
            };

            var education2 = new JobSeekerEducation
            {
                JobSeekerID = 2,
                Degree = "MSc Computer Science",
                Institution = "ABC University",
                Location = "Town",
                StartDate = new DateTime(2020, 9, 1),
                EndDate = new DateTime(2022, 6, 1),
                Description = "Master's degree in computer science",
                GPA = 4.0
            };

            await educationRepository.Add(education1);
            await educationRepository.Add(education2);

            // Act
            var result = await educationRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllJobSeekerEducations_Fail()
        {
            // Act
            var result = await educationRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

    }
}
