using System.Security.Claims;
using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.AuthHandlers
{
    public class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, BaseApiResponse<object>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;
        private readonly IUserRepository _userRepository;

        public RefreshTokenCommandHandler(
            IAuthenticationService authenticationService,
            ILogger<RefreshTokenCommandHandler> logger,
            IUserRepository userRepository
        )
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<BaseApiResponse<object>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Validate and decode the old refresh token to get user information if needed
                if (
                    !_authenticationService.ValidateToken(
                        request.OldAccessToken,
                        out var principal
                    )
                )
                {
                    _logger.LogWarning(
                        "Invalid or expired refresh token: {OldAccessToken}",
                        request.OldAccessToken
                    );
                    return BaseApiResponse<object>.FailureResponse(
                        "Invalid token or token expired."
                    );
                }

                // Retrieve user information from claims if necessary
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userRepository.GetUserByUsernameAsync(userId!);
                if (user == null)
                {
                    _logger.LogWarning(
                        "No user found for provided token: {OldAccessToken}",
                        request.OldAccessToken
                    );
                    return BaseApiResponse<object>.FailureResponse(
                        "Invalid token or token expired."
                    );
                }

                // Generate new tokens
                var accessToken = _authenticationService.GenerateJwtToken(user);
                var refreshToken = _authenticationService.GenerateRefreshToken(user);
                var tokens = new { AccessToken = accessToken, RefreshToken = refreshToken };

                return BaseApiResponse<object>.SuccessResponse(tokens, "Token refresh successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token refresh.");
                return BaseApiResponse<object>.FailureResponse(
                    "An error occurred during the token refresh process."
                );
            }
        }
    }
}