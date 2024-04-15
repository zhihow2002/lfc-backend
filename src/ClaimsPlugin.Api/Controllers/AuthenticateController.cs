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
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                if (token != string.Empty)
                {
                    var response = BaseApiResponse<object>.SuccessResponse(
                        new { Token = token },
                        "Login successful."
                    );
                    return Ok(new { response });
                }
                else
                {
                    var errorResponse = BaseApiResponse<object>.FailureResponse(
                        "Login failed. Unauthorized access."
                    );
                    return Unauthorized(errorResponse);
                }
            }
            catch (UnauthorizedAccessException)
            {
                var errorResponse = BaseApiResponse<object>.FailureResponse(
                    "Login failed. Unauthorized access."
                );
                return Unauthorized(errorResponse);
            }
        }
    }
}
