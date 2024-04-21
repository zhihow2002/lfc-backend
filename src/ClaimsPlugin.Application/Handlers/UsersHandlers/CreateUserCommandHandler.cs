using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Application.Specifications.UsersSpecification;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsPlugin.Application.Handlers.UsersHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, SingleResponse<Guid>>
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IRepositoryWithEvents<User> _userRepositoryWithEvents;
        private readonly IReadRepository<User> _userReadRepository;

        public CreateUserCommandHandler(
            ILogger<CreateUserCommandHandler> logger,
            IRepositoryWithEvents<User> userRepositoryWithEvents,
            IReadRepository<User> userReadRepository
        )
        {
            _logger = logger;
            _userRepositoryWithEvents = userRepositoryWithEvents;
            _userReadRepository = userReadRepository;
        }

        public async Task<SingleResponse<Guid>> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            var user = await _userReadRepository.AnyAsync(
                GetUserSpecification.GetUserByEmail(command.Email),
                cancellationToken
            );

            if (user)
            {
                _logger.LogError("User Exists");
                return await SingleResponse<Guid>.FailAsync("User Exists");
            }

            User newUser = new(command.Username, command.Email, command.Password);

            User result = await _userRepositoryWithEvents.AddAsync(newUser, cancellationToken);

            return await SingleResponse<Guid>.SuccessAsync(result.Id);
        }
    }
}
