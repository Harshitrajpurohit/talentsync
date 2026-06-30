using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Helper;
using TalentSync.Application.DTOs.Auth;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Auth;

namespace TalentSync.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly CookieHelper _cookieHelper;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, CookieHelper cookieHelper)
        {
            _logger = logger;
            _authService = authService;
            _cookieHelper = cookieHelper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync([FromBody]UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating user with Email : {Email}", userRegisterRequestDto.Email);
            UserResponseDto userResponseDto = await _authService.CreateUserAsync(userRegisterRequestDto, cancellationToken);

            return Ok(userResponseDto);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequestdto userLoginRequestdto, CancellationToken cancellationToken) {
            _logger.LogInformation("Try Login user with Email : {Email}", userLoginRequestdto.Email);
            UserLoginResponseDto userLoginResponseDto = await _authService.LoginAsync(userLoginRequestdto, cancellationToken);

            _cookieHelper.SetRefreshTokenCookie(Response, userLoginResponseDto.RefreshToken);

            return Ok(new
            {
                UserId = userLoginResponseDto.UserId,
                Name = userLoginResponseDto.Name,
                Email = userLoginResponseDto.Email,
                Role = userLoginResponseDto.Role,
                Token = userLoginResponseDto.Token,
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token is missing.");
            }

            RefreshTokenResponseDto refreshTokenResponse = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);

            _cookieHelper.SetRefreshTokenCookie(Response, refreshTokenResponse.RefreshToken);

            return Ok(new
            {
                Token = refreshTokenResponse.Token,
                Email = refreshTokenResponse.Email,
                Role = refreshTokenResponse.Role,
            });

            

        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                RefreshToken? refreshTokenResponse = await _authService.LogoutAsync(refreshToken, cancellationToken);
            }

            _cookieHelper.DeleteRefreshTokenCookie(Response);

            return Ok(new {message = "Logout Successfull."});
        }
    }
}
