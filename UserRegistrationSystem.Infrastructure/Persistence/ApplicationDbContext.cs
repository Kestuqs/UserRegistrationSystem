using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistrationSystem.Domain.Models;

namespace UserRegistrationSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vartotojo unikalus username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Vienas User turi vieną Person
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne()
                .HasForeignKey<Person>(p => p.Id);

            //Person - Address kaip owned type
            modelBuilder.Entity<Person>()
                .OwnsOne(p => p.Address);
        }


    }

}
