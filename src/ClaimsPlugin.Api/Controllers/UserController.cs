using System;
using System.Threading.Tasks;
using ClaimsPlugin.Application.Commands.UsersCommands;
using ClaimsPlugin.Application.Dtos;
using ClaimsPlugin.Application.Queries.UsersQueries;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            string userId,
            [FromBody] UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            // Implement your logic to update a user by id using MediatR
            // Example: command.UserId = userId;
            //          var result = await _mediator.Send(command);
            // return Ok(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            // Implement your logic to delete a user by id using MediatR
            // Example: var result = await _mediator.Send(new DeleteUserCommand(id));
            // return Ok(result);
            return Ok();
        }
    }
}
