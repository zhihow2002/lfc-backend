using System.Reflection;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<TokenManager> Tokens => Set<TokenManager>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            // modelBuilder.ApplyConfiguration(new UserConfiguration());
            // modelBuilder.ApplyConfiguration(new TokenManagerConfiguration());
            // modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            // Alternatively, if you have many configurations, you can apply them all at once using:
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Define your seed data here
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        UserId = "admin",
                        PasswordHash = "hashed-password",
                        Email = "test@gmail.com"
                    }
                // Add more users as needed
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    }
}
