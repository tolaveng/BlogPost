using Core.Application.DTOs;
using MongoDB.Bson;

namespace Core.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(ObjectId userId);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<int> CountUsersAsync();
        Task<bool> CreateUserAsyc(UserDto user);

        Task<UserDto> AuthenticateUserAsync(string username, string password);
    }
}
