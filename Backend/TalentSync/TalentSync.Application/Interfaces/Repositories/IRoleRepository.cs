using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<Role?> GetRoleByIdAsync(Guid rId, CancellationToken cancellationToken);
        Task<Role?> GetRoleByRoleNameAsync(RoleName roleName, CancellationToken cancellationToken);
        Task<Role?> GetRoleByIdForUpdateAsync(Guid rId, CancellationToken cancellationToken);
        Task<Role?> GetRoleByRoleNameForUpdateAsync(RoleName roleName, CancellationToken cancellationToken);
        Task<Role> AddRoleAsync(Role role, CancellationToken cancellationToken);
        void DeleteRole(Role role);
        Task<Role?> GetRoleByIdIncludingDeletedAsync(Guid rId, CancellationToken cancellationToken);
        void RestoreDeletedRole(Role role);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
