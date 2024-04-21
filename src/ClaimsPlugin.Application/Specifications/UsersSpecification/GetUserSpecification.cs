using Ardalis.Specification;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Specification;

namespace ClaimsPlugin.Application.Specifications.UsersSpecification
{
    public static class GetUserSpecification
    {
        public static SingleQuerySpecification<User> GetUserById(Guid id)
        {
            return new SingleQuerySpecification<User>(query => query.Where(x => x.Id == id));
        }

        public static SingleQuerySpecification<User> GetUserByUserId(int userId)
        {
            return new SingleQuerySpecification<User>(
                query => query.Where(x => x.UserId == userId)
            );
        }

        public static SingleQuerySpecification<User> GetUserByEmail(string email)
        {
            return new SingleQuerySpecification<User>(query => query.Where(x => x.Email == email));
        }
    }
}
