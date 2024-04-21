using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Application.Specifications.UsersSpecification;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.UsersHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response>
    {
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly IRepositoryWithEvents<User> _userRepositoryWithEvents;

        public DeleteUserCommandHandler(
            ILogger<DeleteUserCommandHandler> logger,
            IRepositoryWithEvents<User> userRepositoryWithEvents
        )
        {
            _logger = logger;
            _userRepositoryWithEvents = userRepositoryWithEvents;
        }

        public async Task<Response> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userRepositoryWithEvents.FirstOrDefaultAsync(
                GetUserSpecification.GetUserById(request.Id),
                cancellationToken
            );

            if (user == null)
            {
                _logger.LogError("User not found");
                return await Response.FailAsync("User not found");
            }

            await _userRepositoryWithEvents.DeleteAsync(user, cancellationToken);
            return await Response.SuccessAsync();
        }
    }
}
