using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.AuthHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, SingleResponse<object>>
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

        public async Task<SingleResponse<object>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authenticationService.AuthenticateAsync(
                    command.Userid,
                    command.Password
                );
                if (user == null)
                {
                    return SingleResponse<object>.FailAsync("Invalid credentials.");
                }

                var accessToken = _authenticationService.GenerateJwtToken(user);
                var refreshToken = _authenticationService.GenerateRefreshToken(user);
                var tokens = new { AccessToken = accessToken, RefreshToken = refreshToken };

                return SingleResponse<object>.SuccessResponse(
                    tokens,
                    "Authentication successful."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred during authentication for user {UserId}",
                    command.Userid
                );
                return SingleResponse<object>.FailureResponse(
                    "An error occurred during the login process."
                );
            }
        }
    }
}
