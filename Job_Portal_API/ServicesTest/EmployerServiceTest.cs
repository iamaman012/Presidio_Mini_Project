using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Job_Portal_API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTest
{
    public class EmployerServiceTest
    { private JobPortalApiContext _context;
        private IRepository<int, Employer> _employerRepo;
        private IRepository<int, User> _userRepo;
        private IEmployer _employerService;

        [SetUp]
        public async Task Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB");
            _context = new JobPortalApiContext(optionsBuilder.Options);
            _employerRepo = new EmployerRepository(_context);
            _userRepo = new UserRepository(_context);
            _employerService = new EmployerService(_employerRepo, _userRepo);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // Test for adding an Employer (pass case)
        [Test]
        public async Task AddEmployerTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();
            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"

            };
            var result = await _employerService.AddEmployerDetails(empDto);
            Assert.NotNull(result);
            Assert.AreEqual(empDto.UserID, result.UserID);
            Assert.AreEqual(empDto.CompanyName, result.CompanyName);
        }
        [Test]
        public void AddEmployerTest_Exception_UserNotFound()
        {
            // Arrange
            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"

            };
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _employerService.AddEmployerDetails(empDto));
            Assert.AreEqual("User not found in the database", ex.Message);
        }
        [Test]
        public async  Task AddEmployerTest_Exception_UserTypeNotAllowed()
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
            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"

            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UserTypeNotAllowedException>(async () => await _employerService.AddEmployerDetails(empDto));

        }
        [Test]
        public async Task AddEmployerTest_Exception_UserAlreadyExist()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var existingEmployer = new Employer
            {
                UserID = 1,
                CompanyName = "Existing Company",
                CompanyDescription = "Existing Description",
                CompanyLocation = "Existing Location"
            };
            await _context.Employers.AddAsync(existingEmployer);
            await _context.SaveChangesAsync();

            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UserAlreadyExistException>(async () => await _employerService.AddEmployerDetails(empDto));
            Assert.AreEqual("Employer Already Exist", ex.Message);
        }

        // Test for updating a company description (pass case)
        [Test]
        public async Task UpdateCompanyDescriptionTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();
            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"

            };
            var newempDto = await _employerService.AddEmployerDetails(empDto);
            var result = await _employerService.UpdateCompanyDescription(newempDto.EmployerID, "LMN");
            Assert.NotNull(result);
            Assert.AreEqual(newempDto.EmployerID, result.EmployerID);
            Assert.AreEqual(result.CompanyDescription, "LMN");
        }
        [Test]
        public void UpdateCompanyDescriptionTest_Exception_UserNotFound()
        {
            // Arrange
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _employerService.UpdateCompanyDescription(99, "LMN"));
            Assert.AreEqual("Employer Not Exist!!", ex.Message);
        }
        // Test for updating a company location (pass case)
        [Test]
        public async Task UpdateCompanyLocationTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"
            };
            var newempDto = await _employerService.AddEmployerDetails(empDto);

            // Act
            var result = await _employerService.UpdateCompanyLocation(newempDto.EmployerID, "LMN");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newempDto.EmployerID, result.EmployerID);
            Assert.AreEqual("LMN", result.CompanyLocation);
        }
        [Test]
        public void UpdateCompanyLocationTest_Exception_EmployerNotFound()
        {
            // Arrange
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _employerService.UpdateCompanyLocation(99, "LMN"));
            Assert.AreEqual("Employer Not Exist!!", ex.Message);
        }
        [Test]
        public async Task UpdateCompanyNameTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"
            };
            var newempDto = await _employerService.AddEmployerDetails(empDto);

            // Act
            var result = await _employerService.UpdateCompanyName(newempDto.EmployerID, "NewCompanyName");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newempDto.EmployerID, result.EmployerID);
            Assert.AreEqual("NewCompanyName", result.CompanyName);
        }
        [Test]
        public void UpdateCompanyNameTest_Exception_EmployerNotFound()
        {
            // Arrange
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _employerService.UpdateCompanyName(99, "NewCompanyName"));
            Assert.AreEqual("Employer Not Exist!!", ex.Message);
        }
        [Test]
        public async Task GetEmployerByIdTest_Pass()
        {
            // Arrange
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.Employer,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var empDto = new AddEmployerDTO
            {
                UserID = 1,
                CompanyName = "ABC",
                CompanyDescription = "XYZ",
                CompanyLocation = "PQR"
            };
            var newempDto = await _employerService.AddEmployerDetails(empDto);

            // Act
            var result = await _employerService.GetEmployerById(newempDto.EmployerID);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newempDto.EmployerID, result.EmployerID);
            Assert.AreEqual(newempDto.UserID, result.UserID);
            Assert.AreEqual(newempDto.CompanyName, result.CompanyName);
        }
        [Test]
        public void GetEmployerByIdTest_Exception_EmployerNotFound()
        {
            // Arrange
            // Act & Assert
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _employerService.GetEmployerById(99));
            Assert.AreEqual("Employer Not Exist!!", ex.Message);
        }











    }
}
