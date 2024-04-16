using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using MediatR;

namespace ClaimsPlugin.Application.Queries.UsersQueries
{
    public class GetUserQuery : IRequest<BaseApiResponse<object>>
    {
        public int Userid { get; }

        public GetUserQuery(int userid)
        {
            Userid = userid;
        }
    }
}
