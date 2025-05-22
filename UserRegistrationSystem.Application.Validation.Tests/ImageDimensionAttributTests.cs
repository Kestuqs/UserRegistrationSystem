using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using UserRegistrationSystem.Application.Validation;

namespace UserRegistrationSystem.Tests
{
    public class ImageDimensionAttributeTests
    {
        private IFormFile CreateTestImage(int width, int height)
        {
            var stream = new MemoryStream();
            using (var image = new Image<Rgba32>(width, height))
            {
                image.SaveAsJpeg(stream);
            }
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "ProfilePicture", "test.jpg");
        }

        [Fact]
        public void IsValid_WithExactDimensions_ShouldPass()
        {
            // Arrange
            var image = CreateTestImage(200, 200);
            var attribute = new ImageDimensionAttribute(200, 200);

            // Act
            var result = attribute.GetValidationResult(image, new ValidationContext(new object()));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithWrongDimensions_ShouldAutoResizeAndPass()
        {
            // Arrange
            var image = CreateTestImage(100, 100); // bus automatiškai perkeista
            var attribute = new ImageDimensionAttribute(200, 200);

            // Act
            var result = attribute.GetValidationResult(image, new ValidationContext(new object()));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidImage_ShouldReturnError()
        {
            // Arrange: sugadintas failas (nepaveikslėlis)
            var invalidStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("nepaveikslėlis turinys"));
            var file = new FormFile(invalidStream, 0, invalidStream.Length, "ProfilePicture", "fake.jpg");

            var attribute = new ImageDimensionAttribute(200, 200);

            // Act
            var result = attribute.GetValidationResult(file, new ValidationContext(new object()));

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Failed to process the image.", result?.ErrorMessage);
        }
    }
}

