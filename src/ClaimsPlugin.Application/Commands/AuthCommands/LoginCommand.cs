using MediatR;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
namespace ClaimsPlugin.Application.Commands.AuthCommands
{
    public class LoginCommand : IRequest<SingleResponse<object>>
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
