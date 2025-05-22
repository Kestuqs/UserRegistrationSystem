using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using UserRegistrationSystem.Application.DTO;
using System.Xml;

namespace UserRegistrationSystem.Tests.UserRegistrationSystem.Application.Dto.Test
{
    public class LoginUserDtoTest
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
        [Fact]
        public void LoginUserDto_WithValidData_ShouldBevalid()
        {
            // Arrange
            var dto = new LoginUserDto
            {
                Username = "testuser",
                Password = "password123"
            };
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void LoginUserDto_WithoutUsername_ShouldBeInvalid()
        {
            // Arrange
            var dto = new LoginUserDto
            {
                Username = null,
                Password = "password123"
            };
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Username"));
        }

        [Fact]
        public void LoginUserDto_WithoutPassword_ShouldBeInvalid()
        {
            // Arrange
            var dto = new LoginUserDto
            {
                Username = "testuser",
                Password = null
            };
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Password"));
        }

        [Fact]
        public void LoginUserDto_WithoutUsernameAndPassword_ShouldBeInvalid()
        {
            // Arrange
            var dto = new LoginUserDto
            {
                Username = null,
                Password = null
            };
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Equal(2, validationResults.Count);
        }
    }
}