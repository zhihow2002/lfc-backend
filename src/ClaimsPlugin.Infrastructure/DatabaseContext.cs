using ClaimsPlugin.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<User>? Users => Set<User>();
        public DbSet<TokenManager> Tokens => Set<TokenManager>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(i =>
            {
                i.ToTable("Users");
                i.HasKey(k => k.Id);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                const string ConnectionStr =
                    "Data Source=ZHIHOW;Initial Catalog=LFC;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }
    }
}
