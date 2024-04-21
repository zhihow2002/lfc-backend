using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Commands;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;

namespace ClaimsPlugin.Application.Commands.UsersCommands
{
    public class DeleteUserCommand : SingleCommand, IRequest<Response>
    {
        public Guid Id { get; set; }
    }
}
