using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistrationSystem.Domain.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
 
        public byte[] ProfilePicture { get; set; } = null!;

        public Address Address { get; set; } = null!;
    }
}
