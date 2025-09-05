using Perfumes.BLL.DTOs.User;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<string> GenerateTokenAsync(UserDto user);
        Task<bool> RevokeTokenAsync(string token);
        Task<UserDto?> GetUserFromTokenAsync(string token);
    }
} 