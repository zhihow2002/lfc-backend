using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Commands;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Application.Commands.UsersCommands
{
    public class CreateUserCommand : SingleCommand, IRequest<SingleResponse<Guid>>
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
