using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ClaimsPlugin.Application.Commands.AuthCommands
{
    public class LoginCommand: IRequest<string>
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
