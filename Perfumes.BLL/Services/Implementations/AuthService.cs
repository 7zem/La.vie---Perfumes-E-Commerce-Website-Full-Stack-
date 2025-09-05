using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using Perfumes.BLL.Configuration;
using Perfumes.BLL.DTOs.User;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.Exceptions;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ILoggingService _loggingService;
        private readonly ICachingService _cachingService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            JwtService jwtService,
            IEmailService emailService,
            ILoggingService loggingService,
            ICachingService cachingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailService = emailService;
            _loggingService = loggingService;
            _cachingService = cachingService;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            try
            {
                _loggingService.LogInformation($"Registration attempt for email: {registerDto.Email}");

                // Check if email already exists
                if (await _unitOfWork.Users.ExistsAsync(u => u.Email == registerDto.Email))
                {
                    _loggingService.LogWarning($"Registration failed - Email already exists: {registerDto.Email}");
                    throw new DuplicateEntityException("User", "Email", registerDto.Email);
                }

                // Create user entity
                var user = _mapper.Map<Perfumes.DAL.Entities.User>(registerDto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                user.Role = "Customer";
                user.IsActive = true;
                user.EmailVerified = false;
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                // Save user
                var created = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Generate verification token
                var verificationToken = Guid.NewGuid().ToString();
                await _cachingService.SetAsync($"email_verification_{verificationToken}", user.UserId, TimeSpan.FromHours(24));

                // Send verification email
                await SendVerificationEmailAsync(user.Email, verificationToken);

                // Map to DTO
                var userDto = _mapper.Map<UserDto>(created);

                // Generate JWT token
                var token = _jwtService.GenerateToken(userDto);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token
                await _cachingService.SetAsync($"refresh_token_{refreshToken}", user.UserId, TimeSpan.FromDays(7));

                _loggingService.LogInformation($"User registered successfully: {user.UserId}");

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    User = userDto,
                    Message = "Registration successful. Please check your email for verification.",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Registration error: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
        {
            try
            {
                _loggingService.LogInformation($"Login attempt for email: {loginDto.Email}");

                // Find user by email
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    _loggingService.LogWarning($"Login failed - User not found: {loginDto.Email}");
                    throw new EntityNotFoundException("User", loginDto.Email);
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _loggingService.LogWarning($"Login failed - Invalid password for: {loginDto.Email}");
                    throw new System.InvalidOperationException("Invalid email or password");
                }

                // Check if account is active
                if (!user.IsActive)
                {
                    _loggingService.LogWarning($"Login failed - Account deactivated: {loginDto.Email}");
                    throw new System.InvalidOperationException("Account is deactivated");
                }

                // Map to DTO
                var userDto = _mapper.Map<UserDto>(user);

                // Generate JWT token
                var token = _jwtService.GenerateToken(userDto);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token
                await _cachingService.SetAsync($"refresh_token_{refreshToken}", user.UserId, TimeSpan.FromDays(7));

                _loggingService.LogInformation($"User logged in successfully: {user.UserId}");

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    User = userDto,
                    Message = "Login successful",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Login error: {ex.Message}", ex);
                throw;
            }
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            var principal = _jwtService.ValidateToken(token);
            return Task.FromResult(principal != null);
        }

        public Task<string> GenerateTokenAsync(UserDto user)
        {
            return Task.FromResult(_jwtService.GenerateToken(user));
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            await _cachingService.RemoveAsync($"refresh_token_{refreshToken}");
            return true;
        }

        public async Task<UserDto?> GetUserFromTokenAsync(string token)
        {
            var userId = _jwtService.GetUserIdFromToken(token);
            if (userId == null) return null;

            var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        private async Task SendVerificationEmailAsync(string email, string token)
        {
            var verificationUrl = $"https://yourdomain.com/verify-email?token={token}";
            var subject = "Verify Your Email Address";
            var htmlBody = $@"
                <h2>Welcome to Perfumes Store!</h2>
                <p>Please click the link below to verify your email address:</p>
                <a href='{verificationUrl}'>{verificationUrl}</a>
                <p>This link will expire in 24 hours.</p>";

            await _emailService.SendEmailAsync(email, subject, htmlBody);
        }
    }
} 