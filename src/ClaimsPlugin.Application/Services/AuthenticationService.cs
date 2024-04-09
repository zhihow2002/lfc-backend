using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClaimsPlugin.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher
        )
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null; // User not found
            }

            // var verificationResult = _passwordHasher.VerifyHashedPassword(
            //     user,
            //     user.PasswordHash,
            //     password
            // );

            // if (verificationResult == PasswordVerificationResult.Success)
            // {
            //     return user; // Authentication successful
            // }
            if (user.PasswordHash == password) // Assuming user.PasswordHash contains the plain text password
            {
                return user; // Authentication successful
            }

            return null; // Authentication failed
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        //    new Claim(ClaimTypes., user.Username)
                        // Add more claims as needed
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration as needed
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
