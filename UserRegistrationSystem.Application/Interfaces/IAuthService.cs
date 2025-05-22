using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Domain.Models;

namespace UserRegistrationSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
        Task<User?> LoginAsync(LoginUserDto loginUserDto);
    }
}
