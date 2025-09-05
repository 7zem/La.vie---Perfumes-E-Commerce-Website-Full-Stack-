using AutoMapper;
using BCrypt.Net;
using Perfumes.BLL.DTOs.User;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.Exceptions;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILoggingService _loggingService;
        private readonly ICachingService _cachingService;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailService emailService,
            ILoggingService loggingService,
            ICachingService cachingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _loggingService = loggingService;
            _cachingService = cachingService;
        }

        // Profile Management
        public async Task<UserDto?> GetProfileAsync(int userId)
        {
            try
            {
                _loggingService.LogInformation($"Getting profile for user: {userId}");
                
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return null;

                var userDto = _mapper.Map<UserDto>(user);

                // Get user statistics
                userDto.TotalOrders = await _unitOfWork.Orders.CountAsync(o => o.UserId == userId);
                userDto.TotalReviews = await _unitOfWork.Reviews.CountAsync(r => r.UserId == userId);
                userDto.WishlistItems = await _unitOfWork.Wishlist.CountAsync(w => w.UserId == userId);
                userDto.TotalSpent = await _unitOfWork.Orders.GetTotalSalesAsync(null, null);

                return userDto;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting profile for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserDto updateDto)
        {
            try
            {
                _loggingService.LogInformation($"Updating profile for user: {userId}");

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                _mapper.Map(updateDto, user);
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cachingService.RemoveAsync($"user_profile_{userId}");

                _loggingService.LogInformation($"Profile updated successfully for user: {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating profile for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                _loggingService.LogInformation($"Changing password for user: {userId}");

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    _loggingService.LogWarning($"Password change failed - Invalid current password for user: {userId}");
                    throw new System.InvalidOperationException("Current password is incorrect");
                }

                // Hash new password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Send email notification
                await SendPasswordChangeNotificationAsync(user.Email);

                _loggingService.LogInformation($"Password changed successfully for user: {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error changing password for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        // Password Reset
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                _loggingService.LogInformation($"Password reset requested for email: {forgotPasswordDto.Email}");

                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
                if (user == null) return false;

                // Generate reset token
                var resetToken = Guid.NewGuid().ToString();
                await _cachingService.SetAsync($"password_reset_{resetToken}", user.UserId, TimeSpan.FromHours(1));

                // Send reset email
                await SendPasswordResetEmailAsync(user.Email, resetToken);

                _loggingService.LogInformation($"Password reset email sent to: {forgotPasswordDto.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in forgot password for {forgotPasswordDto.Email}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                _loggingService.LogInformation($"Password reset attempt with token");

                var userId = await _cachingService.GetAsync<int>($"password_reset_{resetPasswordDto.Token}");
                if (userId == 0) return false;

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                // Hash new password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Remove reset token
                await _cachingService.RemoveAsync($"password_reset_{resetPasswordDto.Token}");

                // Send confirmation email
                await SendPasswordResetConfirmationAsync(user.Email);

                _loggingService.LogInformation($"Password reset successfully for user: {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in password reset: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string token)
        {
            var userId = await _cachingService.GetAsync<int>($"password_reset_{token}");
            return userId != 0;
        }

        // Email Verification
        public async Task<bool> SendEmailVerificationAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                var verificationToken = Guid.NewGuid().ToString();
                await _cachingService.SetAsync($"email_verification_{verificationToken}", userId, TimeSpan.FromHours(24));

                await SendVerificationEmailAsync(user.Email, verificationToken);
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error sending verification email for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            try
            {
                var userId = await _cachingService.GetAsync<int>($"email_verification_{token}");
                if (userId == 0) return false;

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.EmailVerified = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                await _cachingService.RemoveAsync($"email_verification_{token}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error verifying email with token: {ex.Message}", ex);
                throw;
            }
        }

        // Account Management
        public async Task<bool> DeactivateAccountAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deactivating account for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> ReactivateAccountAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error reactivating account for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                if (await _unitOfWork.Users.ExistsAsync(u => u.Email == newEmail && u.UserId != userId))
                    throw new DuplicateEntityException("User", "Email", newEmail);

                user.Email = newEmail;
                user.EmailVerified = false;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating email for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAccountAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting account for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        // Admin Functions
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting all users: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                return user != null ? _mapper.Map<UserDto>(user) : null;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.Role = newRole;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating role for user {userId}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> BlockUserAsync(int userId)
        {
            return await DeactivateAccountAsync(userId);
        }

        public async Task<bool> UnblockUserAsync(int userId)
        {
            return await ReactivateAccountAsync(userId);
        }

        // Private helper methods
        private async Task SendPasswordChangeNotificationAsync(string email)
        {
            var subject = "Password Changed Successfully";
            var htmlBody = @"
                <h2>Password Changed</h2>
                <p>Your password has been changed successfully.</p>
                <p>If you did not make this change, please contact support immediately.</p>";

            await _emailService.SendEmailAsync(email, subject, htmlBody);
        }

        private async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var resetUrl = $"https://yourdomain.com/reset-password?token={token}";
            var subject = "Password Reset Request";
            var htmlBody = $@"
                <h2>Password Reset Request</h2>
                <p>Click the link below to reset your password:</p>
                <a href='{resetUrl}'>{resetUrl}</a>
                <p>This link will expire in 1 hour.</p>
                <p>If you did not request this, please ignore this email.</p>";

            await _emailService.SendEmailAsync(email, subject, htmlBody);
        }

        private async Task SendPasswordResetConfirmationAsync(string email)
        {
            var subject = "Password Reset Successful";
            var htmlBody = @"
                <h2>Password Reset Successful</h2>
                <p>Your password has been reset successfully.</p>
                <p>You can now log in with your new password.</p>";

            await _emailService.SendEmailAsync(email, subject, htmlBody);
        }

        private async Task SendVerificationEmailAsync(string email, string token)
        {
            var verificationUrl = $"https://yourdomain.com/verify-email?token={token}";
            var subject = "Verify Your Email Address";
            var htmlBody = $@"
                <h2>Email Verification</h2>
                <p>Please click the link below to verify your email address:</p>
                <a href='{verificationUrl}'>{verificationUrl}</a>
                <p>This link will expire in 24 hours.</p>";

            await _emailService.SendEmailAsync(email, subject, htmlBody);
        }
    }
} 