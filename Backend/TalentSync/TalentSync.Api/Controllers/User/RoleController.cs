using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        public RoleController(ILogger<RoleController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all roles");
            List<string> roles = await _roleService.GetAllRolesAsync(cancellationToken);
            return Ok(roles);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting role with id {Id}", id);
            RoleResponseDto? role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id {id} not found");
            }

            return Ok(role);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetRoleByRoleNameAsync(RoleName name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting role with name {Name}", name);
            RoleResponseDto? role = await _roleService.GetRoleByRoleNameAsync(name, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with name {name} not found");
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleDTO createRoleDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating role with name {Name}", createRoleDTO.Name);
            RoleResponseDto role = await _roleService.CreateRoleAsync(createRoleDTO, cancellationToken);
            return CreatedAtAction(
                        nameof(GetRoleByIdAsync),
                        new { id = role.Id },
                        role);
        }
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromBody] CreateRoleDTO updateRoleDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating role with id {Id}", id);
            RoleResponseDto role = await _roleService.UpdateRoleAsync(id, updateRoleDTO, cancellationToken);
            _logger.LogInformation("Role with id {Id} updated successfully", id);
            return Ok(role);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteRoleAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting role with id {Id}", id);
            bool result = await _roleService.DeleteRoleAsync(id, cancellationToken);
            if (!result)
            {
                throw new KeyNotFoundException($"Role with id {id} not found");
            }
            _logger.LogInformation("Role with id {Id} deleted successfully", id);
            return NoContent();
        }

        [HttpPost("{id:Guid}/restore")]
        public async Task<IActionResult> RestoreRoleAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring role with id {Id}", id);
            bool result = await _roleService.RestoreRoleAsync(id, cancellationToken);
            if (!result)
            {
                throw new KeyNotFoundException($"Role with id {id} not found");
            }
            _logger.LogInformation("Role with id {Id} restored successfully", id);
            return NoContent();
        }

    }
}
