
using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RepositoryTesting
{
    public class UserRepsitoryTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "<Pending>")]
        private JobPortalApiContext context;
        private IRepository<int,User> userRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            context = new JobPortalApiContext(options);
            userRepository = new UserRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }


        // AddUser Tests
        // AddUser Tests
        [Test]
        public async Task AddUser_Pass()
        {
            // Arrange
            var user = new User
            {
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            // Act
            var result = await userRepository.Add(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public async Task AddUser_Fail()
        {
            // Arrange
            var user = new User
            {
                Email = "abc@gmail.com", // This email already exists
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            await userRepository.Add(user);

            // Act & Assert
            // Adding the same email should throw an exception
            Assert.ThrowsAsync<UserAlreadyExistException>(async () => await userRepository.Add(user));
        }

        [Test]
        public void AddUser_Exception()
        {
            // Arrange
            var user = new User(); // Invalid user object

            // Act & Assert
            // Should throw an exception due to invalid input data
            Assert.ThrowsAsync<DbUpdateException>(async () => await userRepository.Add(user));
        }

        // UpdateUser Tests
        [Test]
        public async Task UpdateUser_Pass()
        {
            // Arrange
            var user = new User
            {
                Email = "update@gmail.com",
                FirstName = "Jane",
                LastName = "Doe",
                ContactNumber = "0123456789",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            var addedUser = await userRepository.Add(user);
            addedUser.FirstName = "Updated";

            // Act
            var result = await userRepository.Update(addedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated", result.FirstName);
        }

        [Test]
        public void UpdateUser_Fail()
        {
            // Arrange
            var user = new User
            {
                UserID = 999,
                Email = "nonexistent@gmail.com",
                FirstName = "Non",
                LastName = "Existent",
                ContactNumber = "0000000000",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            // Act & Assert
            // Should throw an exception because the user does not exist
            Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepository.Update(user));
        }

        
      

        // DeleteUser Tests
        [Test]
        public async Task DeleteUser_Pass()
        {
            // Arrange
            var user = new User
            {
                Email = "delete@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "0987654321",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            await userRepository.Add(user);

            // Act
            var result = await userRepository.DeleteById(user.UserID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public void DeleteUser_Fail()
        {
            // Act & Assert
            // Should throw an exception because the user does not exist
            Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepository.DeleteById(999));
        }

       

        // GetUserById Tests
        [Test]
        public async Task GetUserById_Pass()
        {
            // Arrange
            var user = new User
            {
                Email = "getbyid@gmail.com",
                FirstName = "Get",
                LastName = "ById",
                ContactNumber = "1111111111",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            var addedUser = await userRepository.Add(user);

            // Act
            var result = await userRepository.GetById(addedUser.UserID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public void GetUserById_Fail()
        {
            // Act & Assert
            // Should throw an exception because the user does not exist
            Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepository.GetById(999));
        }

      

        // GetAllUsers Tests
        [Test]
        public async Task GetAllUsers_Pass()
        {
            // Arrange
            var user1 = new User
            {
                Email = "user1@gmail.com",
                FirstName = "User",
                LastName = "One",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            var user2 = new User
            {
                Email = "user2@gmail.com",
                FirstName = "User",
                LastName = "Two",
                ContactNumber = "0987654321",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };

            await userRepository.Add(user1);
            await userRepository.Add(user2);

            // Act
            var result = await userRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllUsers_Empty()
        {
            // Act
            var result = await userRepository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}