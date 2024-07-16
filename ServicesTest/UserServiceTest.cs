using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Job_Portal_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace ServicesTest
{
    public class UserServiceTest
    {
        private JobPortalApiContext _context;
        private IRepository<int, User> _userRepository;
        private IRepository<int, JobSeeker> _jobSeekerRepository;
        private IUser _userService;
        private IToken _tokenService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _context = new JobPortalApiContext(options);
            _userRepository = new UserRepository(_context);
            _jobSeekerRepository = new JobSeekerRepository(_context);
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);
            _tokenService = new TokenService(mockConfig.Object);
            _userService = new UserService(_userRepository, _tokenService, _jobSeekerRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task RegisterUser_ValidUser_Pass()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };

            // Act
            var result = await _userService.RegisterUser(userDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.UserID);
            Assert.AreEqual(userDTO.Email, result.Email);
            Assert.AreEqual(userDTO.FirstName + userDTO.LastName, result.Name);
            Assert.AreEqual(userDTO.ContactNumber, result.ContactNumber);
            Assert.AreEqual(UserType.JobSeeker.ToString(), result.Role);
        }

        [Test]
        public async Task RegisterUser_UserAlreadyExists_Fail()
        {
            // Arrange
            var userDTO1 = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var result1 = await _userService.RegisterUser(userDTO1);


            var userDTO2 = new RegisterUserDTO
            {
                Email = "test@example.com", // Duplicate email
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };

            // Act & Assert
            Assert.ThrowsAsync<UserAlreadyExistException>(async () => await _userService.RegisterUser(userDTO2));
        }

        [Test]
        public async Task LoginUser_ValidCredentials_Pass()
        {
            // Arrange
           
            var userDTO1 = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var user = await _userService.RegisterUser(userDTO1);

            var loginDTO = new LoginUserDTO
            {
                Email = "test@example.com",
                Password = "password"
            };

            // Act
            var result = await _userService.LoginUser(loginDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.UserID);
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Role, result.Role);
           
        }

        [Test]
        public async Task LoginUser_InvalidCredentials_Fail()
        {
            // Arrange
            var userDTO1 = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var user = await _userService.RegisterUser(userDTO1);

            var loginDTO = new LoginUserDTO
            {
                Email = "test@example.com",
                Password = "Invalidpassword"
            };

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await _userService.LoginUser(loginDTO));
        }

        [Test]
        public async Task UpdateUserEmail_ValidIdAndEmail_Pass()
        {
            // Arrange
            var userDTO1 = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var user = await _userService.RegisterUser(userDTO1);

            var newEmail = "newemail@example.com";
            var userId = user.UserID;

            // Act
            var result = await _userService.UpdateUserEmail(userId, newEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserID);
            Assert.AreEqual(newEmail, result.Email);
        }

        [Test]
        public void UpdateUserEmail_InvalidUserId_Fail()
        {
            // Arrange
            var invalidUserId = 999; // Non-existent user ID
            var newEmail = "newemail@example.com";

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.UpdateUserEmail(invalidUserId, newEmail));
        }

        [Test]
        public async Task DeleteUserById_ValidId_Pass()
        {
            // Arrange
            var userDTO1 = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var user = await _userService.RegisterUser(userDTO1);

            var userId = user.UserID;

            // Act
            var result = await _userService.DeleteUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserID);
        }

        [Test]
        public void DeleteUserById_InvalidId_Fail()
        {
            // Arrange
            var invalidUserId = 999; // Non-existent user ID

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.DeleteUserById(invalidUserId));
        }
        [Test]
        public async Task ChangePassword_ValidIdAndCredentials_Pass()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "oldpassword"
            };
            var registeredUser = await _userService.RegisterUser(userDTO);
            var userId = registeredUser.UserID;
            var oldPassword = "oldpassword";
            var newPassword = "newpassword";
            var confirmPassword = "newpassword";

            // Act
            var result = await _userService.ChangePassword(userId, oldPassword, newPassword, confirmPassword);

            // Assert
            Assert.AreEqual("Password Changed Successfully", result);
        }

        [Test]
        public void ChangePassword_InvalidId_Fail()
        {
            // Arrange
            var invalidUserId = 999; // Assuming a non-existent user ID
            var oldPassword = "oldpassword";
            var newPassword = "newpassword";
            var confirmPassword = "newpassword";

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.ChangePassword(invalidUserId, oldPassword, newPassword, confirmPassword));
        }

        [Test]
        public async Task ChangePassword_PasswordMismatch_Fail()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "oldpassword"
            };
            var registeredUser = await _userService.RegisterUser(userDTO);
            var userId = registeredUser.UserID;
            var oldPassword = "oldpassword";
            var newPassword = "newpassword";
            var confirmPassword = "wrongpassword"; // Mismatched confirm password

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _userService.ChangePassword(userId, oldPassword, newPassword, confirmPassword));
        }

        [Test]
        public async Task ChangePassword_InvalidOldPassword_Fail()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "oldpassword"
            };
            var registeredUser = await _userService.RegisterUser(userDTO);
            var userId = registeredUser.UserID;
            var oldPassword = "wrongpassword"; // Incorrect old password
            var newPassword = "newpassword";
            var confirmPassword = "newpassword";

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await _userService.ChangePassword(userId, oldPassword, newPassword, confirmPassword));
        }

        [Test]
        public async Task GetUserById_ValidId_Pass()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var registeredUser = await _userService.RegisterUser(userDTO);
            var userId = registeredUser.UserID;

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserID);
            Assert.AreEqual(userDTO.Email, result.Email);
            Assert.AreEqual(userDTO.FirstName + userDTO.LastName, result.Name);
            Assert.AreEqual(userDTO.ContactNumber, result.ContactNumber);
            Assert.AreEqual(UserType.JobSeeker.ToString(), result.Role);
        }

        [Test]
        public void GetUserById_InvalidId_Fail()
        {
            // Arrange
            var invalidUserId = 999; // Assuming a non-existent user ID

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.GetUserById(invalidUserId));
        }
        [Test]
        public async Task LoginUser_UnauthorizedException_Fail()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.JobSeeker.ToString(),
                ContactNumber = "1234567890",
                Password = "password"
            };
            var registeredUser = await _userService.RegisterUser(userDTO); // Register a user

            var loginDTO = new LoginUserDTO
            {
                Email = "test@example.com",
                Password = "InvalidPassword" // Incorrect password
            };

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await _userService.LoginUser(loginDTO));
        }

        [Test]
        public async Task RegisterUser_ArgumentException_Fail()
        {
            // Arrange
            var invalidUserDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = "InvalidUserType", // Invalid user type
                ContactNumber = "1234567890",
                Password = "password"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterUser(invalidUserDTO));
        }





    }
}
