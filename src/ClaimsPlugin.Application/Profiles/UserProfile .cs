using AutoMapper;
using ClaimsPlugin.Application.Dtos.UserDto;
using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            // Define other mappings as needed
        }
    }
}
