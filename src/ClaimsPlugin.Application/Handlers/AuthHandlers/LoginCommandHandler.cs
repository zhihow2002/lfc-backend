using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Application.Interfaces;
using ClaimsPlugin.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.AuthHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IAuthenticationService authenticationService,
            ILogger<LoginCommandHandler> logger
        )
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authenticationService.AuthenticateAsync(
                    command.Userid,
                    command.Password
                );
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }

                // Generate JWT token
                return _authenticationService.GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred during authentication for user {UserId}",
                    command.Userid
                );
                return ""; // Use a more specific error status code
            }
        }
    }
}
