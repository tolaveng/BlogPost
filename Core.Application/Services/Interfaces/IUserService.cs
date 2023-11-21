using Core.Application.DTOs;

namespace Core.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<int> CountUsersAsync();
        Task<bool> CreateUserAsyc(UserDto user);

        Task<UserDto> SignInAsync(string username, string password);
    }
}
