using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Domain.Models;
using UserRegistrationSystem.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using UserRegistrationSystem.Application.Interfaces;
using UserRegistrationSystem.Application.Validation;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace UserRegistrationSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                _logger.LogWarning("Username {Username} is already taken", dto.Username);
                return false;
            }

            if (!AddressValidator.IsValidAddress(dto, out var addressError))
            {
                _logger.LogWarning("Address validation failed: {Message}", addressError);
                return false;
            }

            var salt = GenerateSalt();
            var hash = HashPassword(dto.Password, salt);

            byte[] profilePictureBytes = null!;
            if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
            {
                profilePictureBytes = await ProcessProfilePicture(dto.ProfilePicture);
                if (profilePictureBytes == null)
                {
                    return false;
                }
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User",
                Person = new Person
                {
                    Id = Guid.NewGuid(),
                    PersonalCode = dto.PersonalCode,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    ProfilePicture = profilePictureBytes,
                    Address = new Address
                    {
                        City = dto.City,
                        Street = dto.Street,
                        ZipCode = dto.ZipCode,
                        ApartmentNumber = dto.ApartmentNumber,
                        HouseNumber = dto.HouseNumber
                    }
                }
            };

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user to database");
                return false;
            }

            return true;
        }

        public async Task<User?> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                _logger.LogInformation("User {Username} not found", dto.Username);
                return null;
            }

            var hash = HashPassword(dto.Password, user.PasswordSalt);
            if (hash != user.PasswordHash)
            {
                _logger.LogWarning("Invalid password for user {Username}", dto.Username);
                return null;
            }

            return user;
        }

        private async Task<byte[]> ProcessProfilePicture(IFormFile profilePicture)
        {
            try
            {
                using (var image = Image.Load(profilePicture.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new Size(200, 200)));
                    using (var memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, new JpegEncoder());
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing profile picture");
                return null;
            }
        }

        private static string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }
    }
}
