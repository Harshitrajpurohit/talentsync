using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Enums.User;
using Microsoft.AspNetCore.Authorization;

namespace TalentSync.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all users with pagination: PageNumber={PageNumber}, PageSize={PageSize}", paginationRequest.PageNumber, paginationRequest.PageSize);
            PaginationResponse<UserWithRolesResponseDto> response = await _userService.GetAllUsersAsync(paginationRequest, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);
            UserResponseDto userResponseDto = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(userResponseDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);
            UserResponseDto userResponseDto = await _userService.UpdateUserAsync(id, updateUserDTO, cancellationToken);
            return Ok(userResponseDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            bool isDeleted = await _userService.DeleteUserAsync(id, cancellationToken);

            if (isDeleted)
            {
                _logger.LogInformation(
                    "User deleted successfully. UserId={UserId}",
                    id);
                return NoContent();
            }

            _logger.LogInformation(
                        "User is Not Deleted. UserId={UserId}",
                        id);
            throw new InvalidOperationException("User is Not Deleted");
        }

        // Restore User
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreUserAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring user with ID: {UserId}", id);
            UserResponseDto userResponseDto = await _userService.RestoreUserAsync(id, cancellationToken);
            return Ok(userResponseDto);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeUserStatusAsync(Guid id, [FromQuery] UserStatus newStatus, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Changing status of user with ID: {UserId} to {NewStatus}", id, newStatus);
            UserResponseDto userResponseDto = await _userService.ChangeUserStatusAsync(id, newStatus, cancellationToken);

            return Ok(userResponseDto);
        }
    }

}