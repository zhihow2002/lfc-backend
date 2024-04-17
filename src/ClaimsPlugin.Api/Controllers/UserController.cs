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
        public async Task<BaseApiResponse<UserReadDto>> GetUser(int userId,CancellationToken cancellationToken)
        {
            // Implement your logic to fetch a user by id using MediatR
            // Example: var user = await _mediator.Send(new GetUserQuery(id));
            // return Ok(user);
            return await _mediator.Send(new GetUserQuery(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            // Implement your logic to create a user using MediatR
            // Example: var result = await _mediator.Send(command);
            // return Ok(result);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            string userId,
            [FromBody] UpdateUserCommand command
        )
        {
            // Implement your logic to update a user by id using MediatR
            // Example: command.UserId = userId;
            //          var result = await _mediator.Send(command);
            // return Ok(result);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Implement your logic to delete a user by id using MediatR
            // Example: var result = await _mediator.Send(new DeleteUserCommand(id));
            // return Ok(result);
            return Ok();
        }
    }
}
