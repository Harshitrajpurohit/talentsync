using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Auth;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.Auth;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto> CreateUserAsync(UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken);
        Task<RefreshToken?> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
        Task<UserLoginResponseDto> LoginAsync(UserLoginRequestdto userLoginRequestdto, CancellationToken cancellationToken);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
