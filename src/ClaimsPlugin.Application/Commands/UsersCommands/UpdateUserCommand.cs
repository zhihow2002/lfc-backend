using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Commands;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;

namespace ClaimsPlugin.Application.Commands.UsersCommands
{
    public class UpdateUserCommand : SingleCommand, IRequest<Response>
    {
        public Guid Id { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
