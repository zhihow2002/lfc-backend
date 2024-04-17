using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Application.Interfaces;
using ClaimsPlugin.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;

namespace ClaimsPlugin.Application.Handlers.AuthHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseApiResponse<object>>
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

        public async Task<BaseApiResponse<object>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authenticationService.AuthenticateAsync(
                    command.Userid,
                    command.Password
                );
                if (user == null)
                {
                    return BaseApiResponse<object>.FailureResponse("Invalid credentials.");
                }

                var accessToken = _authenticationService.GenerateJwtToken(user);
                var refreshToken = _authenticationService.GenerateRefreshToken(user);
                var tokens = new { AccessToken = accessToken, RefreshToken = refreshToken };

                return BaseApiResponse<object>.SuccessResponse(
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
                return BaseApiResponse<object>.FailureResponse(
                    "An error occurred during the login process."
                );
            }
        }
    }
}
