using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;

namespace UserRegistrationSystem.Application.Validation
{
    public static class AddressValidator
    {
        public static bool IsValidAddress(RegisterUserDto dto, out string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(dto.City))
            {
                errorMessage = "City is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.Street))
            {
                errorMessage = "Street is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.ZipCode))
            {
                errorMessage = "ZipCode is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.HouseNumber))
            {
                errorMessage = "House number is required.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
