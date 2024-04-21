using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Versioning.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();

    [NonAction]
    public Guid? GetRoleIdFromHeader()
    {
        bool result = Guid.TryParse(HttpContext.Request.Headers["RoleName"], out Guid roleId);

        return result ? roleId : null;
    }
}
