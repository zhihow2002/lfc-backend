using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
namespace ClaimsPlugin.Application.Commands.AuthCommands
{
    public class LoginCommand : IRequest<BaseApiResponse<object>>
    {
        public string Userid { get; }
        public string Password { get; }

        public LoginCommand(string userid, string password)
        {
            Userid = userid;
            Password = password;
        }
    }
}
