using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistrationSystem.Domain.Models
{
    public class Address
    {
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string? ApartmentNumber { get; set; } = null!;
    }
}
