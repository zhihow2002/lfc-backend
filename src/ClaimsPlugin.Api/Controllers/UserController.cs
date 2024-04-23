using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Application.Dtos.UserDto;
using ClaimsPlugin.Application.Queries.UsersQueries;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator =
            mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<BaseApiResponse<UserReadDto>> GetUser(
            int userId,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(new GetUserQuery(userId), cancellationToken);
        }

        [HttpPost]
        public async Task<SingleResponse<Guid>> CreateUser(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPut]
        public async Task<Response> UpdateUser(
            [FromBody] UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpDelete]
        public async Task<Response> DeleteUser(
            [FromBody] DeleteUserCommand command,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
