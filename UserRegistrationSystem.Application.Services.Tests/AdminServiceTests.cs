using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Application.Interfaces;
using UserRegistrationSystem.Domain.Models;
using Xunit;

namespace UserRegistrationSystem.Tests
{
    public class AdminServiceTests
    {
        private readonly Mock<IAdminService> _adminServiceMock;

        public AdminServiceTests()
        {
            _adminServiceMock = new Mock<IAdminService>();
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Username = "user1" },
                new User { Id = Guid.NewGuid(), Username = "user2" }
            };

            _adminServiceMock.Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _adminServiceMock.Object.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "user1" };

            _adminServiceMock.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _adminServiceMock.Object.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result!.Id);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();

            _adminServiceMock.Setup(service => service.UpdateUserRoleAsync(userId, "Admin"))
                .ReturnsAsync(true);

            var result = await _adminServiceMock.Object.UpdateUserRoleAsync(userId, "Admin");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();

            _adminServiceMock.Setup(service => service.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            var result = await _adminServiceMock.Object.DeleteUserAsync(userId);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();
            var dto = new UpdateUserDto
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@example.com"
            };

            _adminServiceMock.Setup(service => service.UpdateUserAsync(userId, dto))
                .ReturnsAsync(true);

            var result = await _adminServiceMock.Object.UpdateUserAsync(userId, dto);

            Assert.True(result);
        }
    }
}
