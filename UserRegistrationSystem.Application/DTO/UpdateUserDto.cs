using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistrationSystem.Application.DTO
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = default!;
    }
}
