using ClaimsPlugin.Application.Commands.AuthCommands;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ClaimsPlugin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        [AllowAnonymous]
        [OpenApiOperation("Login To get Access Token and Refresh Token", "")]
        public async Task<BaseApiResponse<object>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPost("refresh")]
        [OpenApiOperation("Get Refresh Token", "")]
        public async Task<BaseApiResponse<object>> RefreshToken(
            [FromBody] RefreshTokenCommand command,
            CancellationToken cancellationToken
        )
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
