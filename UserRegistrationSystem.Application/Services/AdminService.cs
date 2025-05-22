using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Application.Interfaces;
using UserRegistrationSystem.Domain.Models;
using UserRegistrationSystem.Infrastructure.Persistence;

namespace UserRegistrationSystem.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Person)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Person)
                .ThenInclude(p => p.Address)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateUserRoleAsync(Guid id, string newRole)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null || user.Person == null)
                return false;

            // Atnaujiname naudotojo informaciją
            user.Person.Email = dto.Email;
            user.Person.FirstName = dto.FirstName;
            user.Person.LastName = dto.LastName;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
