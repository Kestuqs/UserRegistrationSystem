using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Domain.Models;
using UserRegistrationSystem.Application.DTO;

namespace UserRegistrationSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserRoleAsync(Guid id, string newRole);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto);
    }
}
