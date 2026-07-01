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
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {

            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationResponse<UserWithRolesResponseDto> response = await _userService.GetAllUsersAsync(paginationRequest, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            UserResponseDto userResponseDto = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(userResponseDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            UserResponseDto userResponseDto = await _userService.UpdateUserAsync(id, updateUserDTO, cancellationToken);
            return Ok(userResponseDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _userService.DeleteUserAsync(id, cancellationToken);

            if (isDeleted)
            {

                return NoContent();
            }

            throw new InvalidOperationException("User is Not Deleted");
        }

        // Restore User
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreUserAsync(Guid id, CancellationToken cancellationToken)
        {
            UserResponseDto userResponseDto = await _userService.RestoreUserAsync(id, cancellationToken);
            return Ok(userResponseDto);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeUserStatusAsync(Guid id, [FromQuery] UserStatus newStatus, CancellationToken cancellationToken)
        {
            UserResponseDto userResponseDto = await _userService.ChangeUserStatusAsync(id, newStatus, cancellationToken);

            return Ok(userResponseDto);
        }
    }

}