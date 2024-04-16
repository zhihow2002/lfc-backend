using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;

namespace ClaimsPlugin.Application.Commands.AuthCommands
{
    public class RefreshTokenCommand : IRequest<BaseApiResponse<object>>
    {
        public string OldRefreshToken { get; set; }

        public RefreshTokenCommand(string oldRefreshToken)
        {
            OldRefreshToken = oldRefreshToken;
        }
    }
}
