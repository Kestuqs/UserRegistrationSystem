using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Domain.Models;
using UserRegistrationSystem.Application.Services;
using System.Security.Claims;

namespace UserRegistrationSystem.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
