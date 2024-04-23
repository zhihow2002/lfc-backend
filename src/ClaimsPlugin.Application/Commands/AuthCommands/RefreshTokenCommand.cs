using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;

namespace ClaimsPlugin.Application.Commands.AuthCommands
{
    public class RefreshTokenCommand : IRequest<BaseApiResponse<object>>
    {
        public string OldAccessToken { get; set; }

        public RefreshTokenCommand(string oldAccessToken)
        {
            OldAccessToken = oldAccessToken;
        }
    }
}
