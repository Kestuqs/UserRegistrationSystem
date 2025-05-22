using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Application.Validation;
using Xunit;

namespace UserRegistrationSystem.Tests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void IsValidAddress_WithValidFields_ShouldReturnTrue()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12345",
                HouseNumber = "10"
            };

            // Act
            var isValid = AddressValidator.IsValidAddress(dto, out string? errorMessage);

            // Assert
            Assert.True(isValid);
            Assert.Null(errorMessage);
        }

        [Theory]
        [InlineData(null, "Gedimino", "12345", "10", "City is required.")]
        [InlineData("Vilnius", null, "12345", "10", "Street is required.")]
        [InlineData("Vilnius", "Gedimino", null, "10", "ZipCode is required.")]
        [InlineData("Vilnius", "Gedimino", "12345", null, "House number is required.")]
        public void IsValidAddress_WithMissingFields_ShouldReturnFalse(
            string city, string street, string zipCode, string houseNumber, string expectedError)
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                City = city,
                Street = street,
                ZipCode = zipCode,
                HouseNumber = houseNumber
            };

            // Act
            var isValid = AddressValidator.IsValidAddress(dto, out string? errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Equal(expectedError, errorMessage);
        }
    }
}
