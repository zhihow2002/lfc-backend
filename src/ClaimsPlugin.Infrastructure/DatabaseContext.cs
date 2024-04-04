using ClaimsPlugin.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Users = Set<User>();
            Tokens = Set<TokenManager>();
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<TokenManager> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string mySqlConnectionStr =
                    "Data Source=ZHIHOW;Initial Catalog=LFC;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
                optionsBuilder.UseSqlServer(mySqlConnectionStr);
            }
        }
    }
}
