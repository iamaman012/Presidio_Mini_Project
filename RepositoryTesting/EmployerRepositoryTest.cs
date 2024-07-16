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
    public class EmployerRepositoryTest
    {
        private JobPortalApiContext context;
        private IRepository<int, Employer> employerRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            employerRepository = new EmployerRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        // AddEmployer Tests
        [Test]
        public async Task AddEmployer_Pass()
        {
            // Arrange
            var employer = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            // Act
            var result = await employerRepository.Add(employer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employer.UserID, result.UserID);
        }

        [Test]
        public async Task AddEmployer_Fail()
        {
            // Arrange
            var employer = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            // Adding the employer the first time
            await employerRepository.Add(employer);

            // Act & Assert
            // Adding the same employer should fail if there's a constraint violation
            Assert.ThrowsAsync<UserAlreadyExistException>(async () => await employerRepository.Add(employer));
        }

        

        // UpdateEmployer Tests
        [Test]
        public async Task UpdateEmployer_Pass()
        {
            // Arrange
            var employer = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            var addedEmployer = await employerRepository.Add(employer);
            addedEmployer.UserID = 2;

            // Act
            var result = await employerRepository.Update(addedEmployer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.UserID);
        }

        [Test]
        public async Task UpdateEmployer_Fail()
        {
            // Arrange
            var employer = new Employer
            {   EmployerID=99,
                UserID = 100,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await employerRepository.Update(employer));
        }

       

        // DeleteEmployer Tests
        [Test]
        public async Task DeleteEmployer_Pass()
        {
            // Arrange
            var employer = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            var addedEmployer = await employerRepository.Add(employer);

            // Act
            var result = await employerRepository.DeleteById(addedEmployer.EmployerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employer.UserID, result.UserID);
        }

        [Test]
        public async Task DeleteEmployer_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await employerRepository.DeleteById(999));
        }

       

        // GetEmployerById Tests
        [Test]
        public async Task GetEmployerById_Pass()
        {
            // Arrange
            var employer = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            var addedEmployer = await employerRepository.Add(employer);

            // Act
            var result = await employerRepository.GetById(addedEmployer.EmployerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employer.UserID, result.UserID);
        }

        [Test]
        public async Task GetEmployerById_Fail()
        {
            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await employerRepository.GetById(999));
        }

       

        // GetAllEmployers Tests
        [Test]
        public async Task GetAllEmployers_Pass()
        {
            // Arrange
            var employer1 = new Employer
            {
                UserID = 1,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            var employer2 = new Employer
            {
                UserID = 2,
                CompanyName = "ABC Company",
                CompanyDescription = "This is a test company",
                CompanyLocation = "Test City",

            };

            await employerRepository.Add(employer1);
            await employerRepository.Add(employer2);

            // Act
            var result = await employerRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllEmployers_Fail()
        {
            // Act
            var result = await employerRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        
    }
}
