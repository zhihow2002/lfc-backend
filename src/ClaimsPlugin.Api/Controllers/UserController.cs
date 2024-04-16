using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
    }
}
