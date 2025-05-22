using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.Validation;

 namespace UserRegistrationSystem.Application.DTO
 {
        public class RegisterUserDto
        {
            [Required]
            [MinLength(3), MaxLength(20)]
            public string Username { get; set; } = null!;

            [Required]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            public string Password { get; set; } = null!;

            [Required]
            public string FirstName { get; set; } = null!;

            [Required]
            public string LastName { get; set; } = null!;

            [Required]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal code must be 11 digits")]
            public string PersonalCode { get; set; } = null!;

            [Required]
            [Phone]
            public string PhoneNumber { get; set; } = null!;

            [Required(ErrorMessage = "Būtina įkelti nuotrauką")]
            [ImageDimension(200, 200)]
            public IFormFile ProfilePicture { get; set; } = null!;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [Required]
            public string ApartmentNumber { get; set; } = null!;

            [Required]
            public string HouseNumber { get; set; } = null!;

            [Required]
            public string City { get; set; } = null!;

            [Required]
            public string Street { get; set; } = null!;

            [Required]
            [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip code must be 5 digits")]
            public string ZipCode { get; set; } = null!;
        }
 }