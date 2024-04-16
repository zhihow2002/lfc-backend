using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ClaimsPlugin.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthenticationService> logger
        )
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public string GenerateRefreshToken(User user)
        {
            // Example of generating a simple unique identifier as a refresh token
            return Guid.NewGuid().ToString();
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
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        //new Claim(ClaimTypes.Name, user.UserId),
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

        public async Task<string> RefreshTokenAsync(string oldToken)
        {
            // Validate the existing token
            if (
                string.IsNullOrEmpty(oldToken)
                || !ValidateToken(oldToken, out ClaimsPrincipal principal)
            )
            {
                return null;
            }

            // Assuming the NameIdentifier claim is the user's identifier
            var userId = principal
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            // Retrieve user based on userId
            var user = await _userRepository.GetUserByUsernameAsync(userId);
            ;
            if (user == null)
            {
                return null;
            }

            // Generate a new JWT token for the user
            return GenerateJwtToken(user);
        }

        public bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                principal = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken validatedToken
                );

                bool isValid =
                    validatedToken is JwtSecurityToken jwtSecurityToken
                    && jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256Signature,
                        StringComparison.InvariantCultureIgnoreCase
                    );

                if (!isValid)
                {
                    _logger.LogWarning("Token validation failed. Token: {Token}", token);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Token validation failed with exception. Token: {Token}",
                    token
                );
                return false;
            }
        }
    }
}
