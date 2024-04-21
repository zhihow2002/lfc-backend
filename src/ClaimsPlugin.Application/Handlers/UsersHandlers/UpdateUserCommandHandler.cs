using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Application.Specifications.UsersSpecification;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.UsersHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response>
    {
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IRepositoryWithEvents<User> _userRepositoryWithEvents;

        public UpdateUserCommandHandler(
            ILogger<UpdateUserCommandHandler> logger,
            IRepositoryWithEvents<User> userRepositoryWithEvents
        )
        {
            _logger = logger;
            _userRepositoryWithEvents = userRepositoryWithEvents;
        }

        public async Task<Response> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            var user = await _userRepositoryWithEvents.FirstOrDefaultAsync(
                GetUserSpecification.GetUserByUserId(command.UserId),
                cancellationToken
            );

            if (user == null)
            {
                _logger.LogError("User not found");
                return await Response.FailAsync("User not found");
            }

            user.UpdateUser(command.Username, command.Email, command.Password);

            await _userRepositoryWithEvents.UpdateAsync(user, cancellationToken);

            return await Response.SuccessAsync();
        }
    }
}
