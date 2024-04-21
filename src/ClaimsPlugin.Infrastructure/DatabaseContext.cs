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
                    new
                    {
                        Id = Guid.NewGuid(),
                        UserId = 1,
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        PasswordHash = "admin"
                    }
                );

            // Set UserId property to be generated on add
            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=LFC;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True",
                    b => b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)
                );
            }
        }
    }
}
