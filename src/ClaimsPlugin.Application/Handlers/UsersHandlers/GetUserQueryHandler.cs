using ClaimsPlugin.Application.Dtos;
using ClaimsPlugin.Application.Queries.UsersQueries;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.UsersHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, BaseApiResponse<UserReadDto>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            IUserRepository userRepository
        )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<BaseApiResponse<UserReadDto>> Handle(
            GetUserQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Retrieve user from the repository based on the query
                var user = await _userRepository.GetUserByIdAsync(query.Userid);

                // Check if the user exists
                if (user == null)
                {
                    return BaseApiResponse<UserReadDto>.FailureResponse(
                        $"User with ID {query.Userid} not found."
                    );
                }

                // Map the user entity to a response DTO
                var userResponse = new UserReadDto
                {
                    Id = user.Id,
                    UserId = user.UserId,
                    Username = user.UserName,
                    Email = user.Email,
                };

                return BaseApiResponse<UserReadDto>.SuccessResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user with ID {UserId}.", query.Userid);
                return BaseApiResponse<UserReadDto>.FailureResponse(
                    "An error occurred while processing the request."
                );
            }
        }
    }
}
