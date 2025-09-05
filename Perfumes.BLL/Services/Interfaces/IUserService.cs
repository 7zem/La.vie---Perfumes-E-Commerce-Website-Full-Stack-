using Perfumes.BLL.DTOs.User;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IUserService
    {
        // Profile Management
        Task<UserDto?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UpdateUserDto updateDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

        // Password Reset
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> VerifyResetTokenAsync(string token);

        // Email Verification
        Task<bool> SendEmailVerificationAsync(int userId);
        Task<bool> VerifyEmailAsync(string token);

        // Account Management
        Task<bool> DeactivateAccountAsync(int userId);
        Task<bool> ReactivateAccountAsync(int userId);
        Task<bool> UpdateEmailAsync(int userId, string newEmail);
        Task<bool> DeleteAccountAsync(int userId);

        // Admin Functions
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserRoleAsync(int userId, string newRole);
        Task<bool> BlockUserAsync(int userId);
        Task<bool> UnblockUserAsync(int userId);
    }
} 