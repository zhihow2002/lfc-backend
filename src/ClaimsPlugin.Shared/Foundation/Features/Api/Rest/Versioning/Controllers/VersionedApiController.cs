using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Versioning.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Api.Rest.Versioning.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}
