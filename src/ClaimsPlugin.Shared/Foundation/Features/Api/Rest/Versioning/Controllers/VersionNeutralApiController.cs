using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Versioning.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Api.Rest.Versioning.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController
{
}
