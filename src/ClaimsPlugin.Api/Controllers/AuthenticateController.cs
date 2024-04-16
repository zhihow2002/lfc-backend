using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        public async Task<BaseApiResponse<object>> Login([FromBody] LoginCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("refresh")]
        public async Task<BaseApiResponse<object>> RefreshToken(
            [FromBody] RefreshTokenCommand command
        )
        {
            return await _mediator.Send(command);
        }
    }
}
