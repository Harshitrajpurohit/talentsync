using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<RoleResponseDto?> GetRoleByIdAsync(Guid rId, CancellationToken cancellationToken);
        Task<RoleResponseDto?> GetRoleByRoleNameAsync(RoleName name, CancellationToken cancellationToken);
        Task<RoleResponseDto> CreateRoleAsync(CreateRoleDTO createRoleDTO, CancellationToken cancellationToken);
        Task<RoleResponseDto> UpdateRoleAsync(Guid rId, CreateRoleDTO updateRoleDTO, CancellationToken cancellationToken);
        Task<bool> DeleteRoleAsync(Guid rId, CancellationToken cancellationToken);
        Task<bool> RestoreRoleAsync(Guid rId, CancellationToken cancellationToken);

    }
}
