using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Infrastructure.DataSeeder
{
    public static class SeedUsers
    {
        public static void Seed(DatabaseContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { UserName = "testuser", PasswordHash = "hashedpassword" } // Hash the password
                    // Add more users as needed
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
