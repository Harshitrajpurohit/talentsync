using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {

        private readonly IUserRoleService _userRoleService;
        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRoleAsync([FromBody] UserRoleRequestDTO createUserRoleDTO, CancellationToken cancellationToken)
        {
            UserRoleResponseDto userRole = await _userRoleService.CreateUserRoleAsync(createUserRoleDTO, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, userRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRoleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            UserRoleResponseDto userRole = await _userRoleService.GetByIdAsync(id, cancellationToken);
            return Ok(userRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserRoleAsync([FromQuery]PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationResponse<UserRoleResponseWithExtraDto> userRoles = await _userRoleService.GetAllUserRolesAsync(paginationRequest, cancellationToken);
            return Ok(userRoles);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRoleAsync(Guid id, [FromBody] UserRoleRequestDTO updateDto, CancellationToken cancellationToken)
        {
            UserRoleResponseDto updatedUserRole = await _userRoleService.UpdateUserRoleAsync(id, updateDto, cancellationToken);
            return Ok(updatedUserRole);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRoleAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userRoleService.DeleteUserRoleAsync(id, cancellationToken);
            return NoContent();
        }

     }
}
