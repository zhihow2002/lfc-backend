using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Application.Dtos;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.ApiReponse;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses;
using MediatR;

namespace ClaimsPlugin.Application.Queries.UsersQueries
{
    public class GetUserQuery : IRequest<BaseApiResponse<UserReadDto>>
    {
        public int Userid { get; }

        public GetUserQuery(int userid)
        {
            Userid = userid;
        }
    }
}
