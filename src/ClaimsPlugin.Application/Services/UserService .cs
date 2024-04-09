using System.Threading.Tasks;
using AutoMapper;
using ClaimsPlugin.Application.Dtos;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Domain.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<UserReadDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<UserReadDto> CreateUserAsync(UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<User>(userCreateDto);
        await _userRepository.CreateUserAsync(user);
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user != null)
        {
            _mapper.Map(userUpdateDto, user);
            _userRepository.UpdateUser(user);
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }
}