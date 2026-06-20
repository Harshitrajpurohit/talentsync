using AutoMapper;
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

        public AuthService(IUserRepository userRepository, 
            IMapper mapper, IPasswordHasher passwordHasher, 
            IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken)
        {
            userRegisterRequestDto.Email = userRegisterRequestDto.Email.Trim().ToLowerInvariant();

            User? user = await _userRepository.GetUserByEmailIncludingDeletedAsync(userRegisterRequestDto.Email, cancellationToken);
                
                if (user is not null)
                {
                        if (user?.IsDeleted == true || user?.Status != UserStatus.Active)
                        {
                            throw new InvalidOperationException("Email id Deactivated, please Restore it.");
                        }
                        throw new InvalidOperationException("Account is Already Available, Please Login.");
                }
            User? existingPhoneUser = await _userRepository.GetUserByPhoneNumberAsync(
                                        userRegisterRequestDto.Phone,
                                        cancellationToken);

            if (existingPhoneUser is not null)
            {
                if (existingPhoneUser?.IsDeleted == true || existingPhoneUser?.Status != UserStatus.Active)
                {
                    throw new InvalidOperationException("Phone Number is Deactivated, please Restore it.");
                }
                throw new InvalidOperationException(
                    "Phone number is already registered.");
            }


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
                return _mapper.Map<UserResponseDto>(userResponse);
            }
            catch
            {
                try
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
                catch
                {
                    // optionally log rollback failure
                }
                throw;
            }

            
        }

        public async Task<RefreshToken?> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? token =  await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken, cancellationToken);

            if (token != null && !token.IsRevoked)
            {
                token.IsRevoked = true;
                token.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return token;
        }

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginRequestdto userLoginRequestdto, CancellationToken cancellationToken)
        {
            userLoginRequestdto.Email = userLoginRequestdto.Email.Trim().ToLowerInvariant();

            User? user = await _userRepository.GetUserByEmailAsync(userLoginRequestdto.Email, cancellationToken);
            if(user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            if(user.IsDeleted || user.Status != UserStatus.Active)
            {
                throw new InvalidOperationException("User is Dactivated, Please restore It.");
            }

            if(!_passwordHasher.VerifyPassword(userLoginRequestdto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdAsync(user.Id, cancellationToken);

            if (userRole == null)
            {
                throw new InvalidOperationException("User Role Not Assigned");
            }
            if (userRole?.Role == null)
            {
                throw new InvalidOperationException(
                    "User role configuration is invalid.");
            }

            var token = _jwtTokenService.GenerateAccessToken(user, userRole.Role.Name);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            RefreshToken refreshTokenEntity =
                new RefreshToken
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    IsRevoked = false
                };

            await _refreshTokenRepository.AddRefreshTokenAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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

            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            if (refreshToken.IsRevoked)
            {
                throw new UnauthorizedAccessException("Refresh token revoked.");
            }
            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token expired.");
            }

            User ?user = refreshToken.User;

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
            if (userRole?.Role == null)
            {
                throw new InvalidOperationException("User role not assigned.");
            }

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

            return new RefreshTokenResponseDto
            {
                Email = user.Email,
                Role = userRole.Role.Name.ToString(),
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
