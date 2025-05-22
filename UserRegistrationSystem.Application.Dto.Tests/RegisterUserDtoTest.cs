using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using UserRegistrationSystem.Application.DTO;
using Xunit;

namespace UserRegistrationSystem.Tests.UserRegistrationSystem.Application.Dto.Test
{
    public class RegisterUserDtoTest
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        private IFormFile CreateFakeImage(int width = 200, int height = 200)
        {
            var stream = new MemoryStream();
            using (var image = new Image<Rgba32>(width, height))
            {
                image.Save(stream, new JpegEncoder());
            }
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "ProfilePicture", "profile.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        [Fact]
        public void RegisterUserDto_WithValidData_ShouldBeValid()
        {
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "StrongPass123",
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                PhoneNumber = "+37060000000",
                Email = "test@example.com",
                ApartmentNumber = "5A",
                HouseNumber = "10",
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12345",
                ProfilePicture = CreateFakeImage(200, 200)
            };
            var result = ValidateModel(dto);
            Assert.Empty(result);
        }

        [Fact]
        public void RegisterUserDto_InvalidEmail_ShouldBeInvalid()
        {
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "StrongPass123",
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                PhoneNumber = "+37060000000",
                Email = "invalid-email",
                ApartmentNumber = "5A",
                HouseNumber = "10",
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12345",
                ProfilePicture = CreateFakeImage(200, 200)
            };
            var result = ValidateModel(dto);
            Assert.Contains(result, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void RegisterUserDto_InvalidZipCode_ShouldBeInvalid()
        {
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "StrongPass123",
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                PhoneNumber = "+37060000000",
                Email = "test@example.com",
                ApartmentNumber = "5A",
                HouseNumber = "10",
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12",
                ProfilePicture = CreateFakeImage(200, 200)
            };
            var result = ValidateModel(dto);
            Assert.Contains(result, r => r.MemberNames.Contains("ZipCode"));
        }

        [Fact]
        public void RegisterUserDto_WrongImageSize_ShouldBeInvalid()
        {
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "StrongPass123",
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                PhoneNumber = "+37060000000",
                Email = "test@example.com",
                ApartmentNumber = "5A",
                HouseNumber = "10",
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12345",
                ProfilePicture = CreateFakeImage(100, 100) // klaidingas dydis
            };

            var result = ValidateModel(dto);


            Assert.Empty(result);
        }
    }
}
