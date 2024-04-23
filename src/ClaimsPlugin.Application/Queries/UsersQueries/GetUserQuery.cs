using ClaimsPlugin.Application.Dtos.UserDto;
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
