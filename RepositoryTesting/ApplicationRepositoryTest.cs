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
    public class ApplicationRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, Application> applicationRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            applicationRepository = new ApplicationRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // Add Tests
        [Test]
        public async Task AddApplication_Pass()
        {
            // Arrange
            var application = new Application
            {
                JobID = 1,
                JobSeekerID = 1,
                Status = "Pending"
            };

            // Act
            var result = await applicationRepository.Add(application);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(application.JobID, result.JobID);
        }

       

        [Test]
        public async Task AddApplication_Exception()
        {
            // Arrange
            var application = new Application(); // Providing empty application to force exception

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await applicationRepository.Add(application));
        }

        // Update Tests
        [Test]
        public async Task UpdateApplication_Pass()
        {
            // Arrange
            var application = new Application
            {
                JobID = 1,
                JobSeekerID = 1,
                Status = "Pending"
            };

            var addedApplication = await applicationRepository.Add(application);
            addedApplication.Status = "Accepted";

            // Act
            var result = await applicationRepository.Update(addedApplication);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Accepted", result.Status);
        }

        [Test]
        public async Task UpdateApplication_Fail()
        {
            // Arrange
            var application = new Application
            {
                ApplicationID = 999, // Providing non-existing ID to force failure
                Status = "NonExistentStatus"
            };

            // Act & Assert
            Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await applicationRepository.Update(application));
        }

       

        // Delete Tests
        [Test]
        public async Task DeleteApplication_Pass()
        {
            // Arrange
            var application = new Application
            {
                JobID = 1,
                JobSeekerID = 1,
                Status = "Pending"
            };

            var addedApplication = await applicationRepository.Add(application);

            // Act
            var result = await applicationRepository.DeleteById(addedApplication.ApplicationID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(application.JobID, result.JobID);
        }

        [Test]
        public async Task DeleteApplication_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await applicationRepository.DeleteById(999));
        }

        // GetById Tests
        [Test]
        public async Task GetApplicationById_Pass()
        {
            // Arrange
            var application = new Application
            {
                JobID = 1,
                JobSeekerID = 1,
                Status = "Pending"
            };

            var addedApplication = await applicationRepository.Add(application);

            // Act
            var result = await applicationRepository.GetById(addedApplication.ApplicationID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(application.JobID, result.JobID);
        }

        [Test]
        public async Task GetApplicationById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await applicationRepository.GetById(999));
        }

        // GetAll Tests
        [Test]
        public async Task GetAllApplications_Pass()
        {
            // Arrange
            var application1 = new Application
            {
                JobID = 1,
                JobSeekerID = 1,
                Status = "Pending"
            };

            var application2 = new Application
            {
                JobID = 2,
                JobSeekerID = 2,
                Status = "Accepted"
            };

            await applicationRepository.Add(application1);
            await applicationRepository.Add(application2);

            // Act
            var result = await applicationRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllApplications_Fail()
        {
            // Act
            var result = await applicationRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
