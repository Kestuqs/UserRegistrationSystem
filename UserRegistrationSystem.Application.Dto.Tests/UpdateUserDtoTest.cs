using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;

namespace UserRegistrationSystem.Tests.UserRegistrationSystem.Application.Dto.Test
{
    public class UpdateUserDtoTest
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
        [Fact]
        public void UpdateUserDto_WithValidData_ShouldBeValid()
        {
            var dto = new UpdateUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
            var result = ValidateModel(dto);
            Assert.Empty(result);
        }
        [Fact]
        public void UpdateUserDto_InvalidEmail_ShouldBeInvalid()
        {
            var dto = new UpdateUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "not-an-email"
            };

            var result = ValidateModel(dto);
            Assert.Contains(result, r => r.MemberNames.Contains("Email"));
        }
    }
}
