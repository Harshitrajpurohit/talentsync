using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Auth;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Auth;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository,
            IMapper mapper, IPasswordHasher passwordHasher,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<AuthService> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Registering new user with email {Email}.",userRegisterRequestDto.Email);

            userRegisterRequestDto.Email = userRegisterRequestDto.Email.Trim().ToLowerInvariant();

            User? user = await _userRepository.GetUserByEmailIncludingDeletedAsync(userRegisterRequestDto.Email, cancellationToken);

            ValidateUserRegistration(user);

            User? existingPhoneUser = await _userRepository.GetUserByPhoneNumberAsync(
                                        userRegisterRequestDto.Phone,
                                        cancellationToken);

            ValidatePhoneRegistration(existingPhoneUser);

            User newUser = _mapper.Map<User>(userRegisterRequestDto);
            newUser.PasswordHash = _passwordHasher.HashPassword(userRegisterRequestDto.Password);
            Role? candidateRole = await _roleRepository.GetRoleByRoleNameAsync(RoleName.Candidate, cancellationToken);

            if (candidateRole == null)
            {
                throw new InvalidOperationException("Default Candidate role is not configured.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {

                User userResponse = await _userRepository.AddUserAsync(newUser, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);
                UserRole newUserRole = new UserRole
                {
                    UserId = userResponse.Id,
                    RoleId = candidateRole.Id
                };
                await _userRoleRepository.AddAsync(newUserRole, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation( "User {UserId} registered successfully with email {Email}.", userResponse.Id, userResponse.Email);

                return _mapper.Map<UserResponseDto>(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user {Email}. Rolling back transaction.", userRegisterRequestDto.Email);
                try
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError( rollbackEx, "Rollback failed while registering user {Email}.", userRegisterRequestDto.Email);
                }
                throw;
            }


        }

        public async Task<RefreshToken?> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? token = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken, cancellationToken);

            if (token != null && !token.IsRevoked)
            {
                token.IsRevoked = true;
                token.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("User {UserId} logged out successfully. Refresh token revoked.", token.UserId);
            }

            return token;
        }

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginRequestdto userLoginRequestdto, CancellationToken cancellationToken)
        {
            userLoginRequestdto.Email = userLoginRequestdto.Email.Trim().ToLowerInvariant();

            User? user = await _userRepository.GetUserByEmailAsync(userLoginRequestdto.Email, cancellationToken);

            ValidateLoginUser(user);

            if (!_passwordHasher.VerifyPassword(userLoginRequestdto.Password, user.PasswordHash))
            {
                _logger.LogWarning(
                    "Failed login attempt for {Email}. Invalid password.",
                    userLoginRequestdto.Email);

                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdAsync(user.Id, cancellationToken);

            ValidateUserRole(userRole);


            var token = _jwtTokenService.GenerateAccessToken(user, userRole.Role.Name);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            RefreshToken refreshTokenEntity =
                new RefreshToken
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };

            await _refreshTokenRepository.AddRefreshTokenAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

             _logger.LogInformation(
                        "User {UserId} logged in successfully with role {Role}.",
                        user.Id,
                        userRole.Role.Name);

            UserLoginResponseDto userLoginResponse = new UserLoginResponseDto
            {
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Token = token,
                Role = userRole.Role.Name.ToString(),
                RefreshToken = newRefreshToken
            };

            return userLoginResponse;

        }

        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshTokenString, CancellationToken cancellationToken)
        {

            RefreshToken? refreshToken = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshTokenString, cancellationToken);

            ValidateRefreshToken(refreshToken);

            User? user = refreshToken.User;

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            if (user.IsDeleted || user.Status != UserStatus.Active)
            {
                throw new UnauthorizedAccessException(
                    "User account is inactive.");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdWithRoleAsync(refreshToken.UserId, cancellationToken);
            
            ValidateUserRole(userRole);

            string newJwtToken = _jwtTokenService.GenerateAccessToken(user, userRole.Role.Name);
            string newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            refreshToken.IsRevoked = true;
            refreshToken.UpdatedAt = DateTime.UtcNow;

            RefreshToken replacementRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddRefreshTokenAsync(replacementRefreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Access token refreshed successfully for user {UserId}.",
                user.Id);

            return new RefreshTokenResponseDto
            {
                Email = user.Email,
                Role = userRole.Role.Name.ToString(),
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }



        // private

        // create user
        private static void ValidateUserRegistration(User? existingUser)
        {
            if (existingUser is null)
                return;

            if (existingUser.IsDeleted || existingUser.Status != UserStatus.Active)
                throw new InvalidOperationException(
                    "This email belongs to a deactivated account. Please restore your account.");

            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        private static void ValidatePhoneRegistration(User? existingPhoneUser)
        {
            if (existingPhoneUser is null)
                return;

            if (existingPhoneUser.IsDeleted || existingPhoneUser.Status != UserStatus.Active)
                throw new InvalidOperationException(
                    "This phone number belongs to a deactivated account. Please restore your account.");

            throw new InvalidOperationException(
                "Phone number is already registered.");
        }

        //  login user
        private static void ValidateLoginUser(User? user)
        {
            if (user is null)
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");

            if (user.IsDeleted || user.Status != UserStatus.Active)
                throw new InvalidOperationException(
                    "Your account is deactivated. Please contact support.");
        }

        private static void ValidateUserRole(UserRole? userRole)
        {
            if (userRole is null)
                throw new InvalidOperationException(
                    "No role is assigned to this user.");

            if (userRole.Role is null)
                throw new InvalidOperationException(
                    "User role configuration is invalid.");
        }

        // refresh tokan 
        private static void ValidateRefreshToken(RefreshToken? refreshToken)
        {
            if (refreshToken is null)
                throw new UnauthorizedAccessException(
                    "Invalid refresh token.");

            if (refreshToken.IsRevoked)
                throw new UnauthorizedAccessException(
                    "Refresh token has been revoked.");

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException(
                    "Refresh token has expired.");
        }
    }
}
