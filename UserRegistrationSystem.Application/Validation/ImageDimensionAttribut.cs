using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace UserRegistrationSystem.Application.Validation
{
    public class ImageDimensionAttribute : ValidationAttribute
    {
        private readonly int _width;
        private readonly int _height;

        public ImageDimensionAttribute(int width, int height)
        {
            _width = width;
            _height = height;
            ErrorMessage = $"The image dimensions must be exactly {_width}x{_height} px.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                try
                {
                    using var image = Image.Load(file.OpenReadStream());

                    if (image.Width != _width || image.Height != _height)
                    {
                        // Pabandom sumažinti arba įtempti
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(_width, _height),
                            Mode = ResizeMode.Pad // Arba Stretch, jei norite įtempti
                        }));

                        // Patikrinam, ar po keitimo dydis atitinka
                        if (image.Width != _width || image.Height != _height)
                        {
                            return new ValidationResult(ErrorMessage);
                        }

          
                    }
                }
                catch
                {
                    return new ValidationResult("Failed to process the image.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
