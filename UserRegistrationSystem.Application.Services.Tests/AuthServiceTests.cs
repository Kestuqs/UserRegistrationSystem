using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Application.Services;
using UserRegistrationSystem.Infrastructure.Persistence;
using UserRegistrationSystem.Domain.Models;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp;
using UserRegistrationSystem.Application.Interfaces;

namespace UserRegistrationSystem.Tests
{
    public class AuthServiceTests
    {
        public readonly ApplicationDbContext _context;
        public readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Inicializuojame In-Memory DbContext kiekvienam testui
            _context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<AuthService>>();
            _authService = new AuthService(_context, logger);
        }

        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unikalus duomenų bazės pavadinimas kiekvienam testui
                .Options;

            return new ApplicationDbContext(options);
        }

        private IFormFile CreateFakeImage(int width, int height)
        {
            var stream = new MemoryStream();
            using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height))
            {
                image.SaveAsJpeg(stream);
            }
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "ProfilePicture", "profile.jpg");
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Username = "testuser",
                Password = "Test123456!",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PersonalCode = "12345678901",
                PhoneNumber = "+37060000000",
                City = "Vilnius",
                Street = "Gedimino",
                ZipCode = "12345",
                ApartmentNumber = "5A",
                HouseNumber = "10",
                ProfilePicture = CreateFakeImage(100, 100) // Nuotrauka bus ištempta iki 200x200
            };

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            Assert.True(result);
            Assert.Single(_context.Users);  // Patikrinti, kad buvo pridėtas tik vienas naudotojas
        }

        [Fact]
        
        public async Task RegisterAsync_WithDuplicateUsername_ShouldReturnFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<AuthService>>();
            var _authService = new AuthService(context, logger);
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "duplicateuser",
                PasswordHash = "dummyhash",      // Būtina, kad būtų sėkmingai išsaugotas įrašas
                PasswordSalt = "dummysalt",      // Būtina
                Role = "User",
                Person = new Person
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    PhoneNumber = "+37060000000",
                    PersonalCode = "12345678901",
                    ProfilePicture = new byte[] {1, 2, 3 }, // Dummy data
                    Address = new Address
                    {
                        City = "Vilnius",
                        Street = "Gedimino",
                        ZipCode = "12345",
                        HouseNumber = "10",
                        ApartmentNumber = "5A"
                    }
                }
            });
            await _context.SaveChangesAsync();

            // Naujas DTO su tuo pačiu naudotojo vardu
            var dto = new RegisterUserDto
            {
                Username = "duplicateuser",
                Password = "password123",
                FirstName = "Jonas",
                LastName = "Jonaitis",
                Email = "jonas@example.com",
                PhoneNumber = "+37061234567",
                PersonalCode = "98765432109",
                City = "Kaunas",
                Street = "Laisvės al.",
                ZipCode = "54321",
                HouseNumber = "15",
                ApartmentNumber = "3B"
            };

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            Assert.False(result);  // Patikrinti, kad grąžinta false, nes vardas jau egzistuoja
        }
    }
}

