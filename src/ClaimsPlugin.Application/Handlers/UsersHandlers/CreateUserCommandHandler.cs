using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Domain.Interfaces;
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
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWithEvents<User> _userRepositoryWithEvents;
        private readonly IReadRepository<User> _userReadRepository;

        public CreateUserCommandHandler(
            ILogger<CreateUserCommandHandler> logger,
            IUserRepository userRepository,
            IRepositoryWithEvents<User> userRepositoryWithEvents,
            IReadRepository<User> userReadRepository
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _userRepositoryWithEvents = userRepositoryWithEvents;
            _userReadRepository = userReadRepository;
        }

        public async Task<SingleResponse<Guid>?> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            var user = await _userRepository.GetUserByUsernameAsync(command.Username);

            // Check if the user exists
            if (user != null)
            {
                return await SingleResponse<Guid>.FailAsync("User exisit");
            }

            return null;
        }
    }
}
