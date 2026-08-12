using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole?> GetByIdAsync(Guid urId, CancellationToken cancellationToken);
        Task<UserRole?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserRole?> GetByUserIdWithRoleAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserRole> AddAsync(UserRole userRole, CancellationToken cancellationToken);
        UserRole Update(UserRole userRole);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<List<UserWithRolesDto>> GetAllUserRolesAsync(UserPaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<int> CountUserRoleAsync(UserPaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<int> CountActiveUserRoleAsync(CancellationToken cancellationToken);
        Task<int> GetCandidateCountAsync(CancellationToken cancellationToken);
        Task<List<UserRoleResponseWithExtraDto>> GetAllUserRoleByRoleAsync(RoleName role, CancellationToken cancellationToken);
    }
}
