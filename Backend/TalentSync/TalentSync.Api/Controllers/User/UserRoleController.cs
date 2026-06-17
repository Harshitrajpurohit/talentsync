using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {

        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;
        public UserRoleController(IUserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRoleAsync([FromBody] UserRoleRequestDTO createUserRoleDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating user role for user {UserId} with role {RoleId}", createUserRoleDTO.UserId, createUserRoleDTO.RoleId);
            UserRoleResponseDto userRole = await _userRoleService.CreateUserRoleAsync(createUserRoleDTO, cancellationToken);
            return CreatedAtAction(
                        nameof(GetUserRoleByIdAsync),
                        new { id = userRole.Id },
                        userRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRoleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting user role with id {Id}", id);
            UserRoleResponseDto userRole = await _userRoleService.GetByIdAsync(id, cancellationToken);
            return Ok(userRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserRoleAsync([FromQuery]PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all user roles");
            PaginationResponse<UserRoleResponseWithExtraDto> userRoles = await _userRoleService.GetAllUserRolesAsync(paginationRequest, cancellationToken);
            return Ok(userRoles);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRoleAsync(Guid id, [FromBody] UserRoleRequestDTO updateDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating user role with id {Id}", id);
            UserRoleResponseDto updatedUserRole = await _userRoleService.UpdateUserRoleAsync(id, updateDto, cancellationToken);
            return Ok(updatedUserRole);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRoleAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting user role with id {Id}", id);
            await _userRoleService.DeleteUserRoleAsync(id, cancellationToken);
            return NoContent();
        }

     }
}
