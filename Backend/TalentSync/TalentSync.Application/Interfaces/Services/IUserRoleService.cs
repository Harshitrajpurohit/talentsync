using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IUserRoleService
    {
        Task<UserRoleResponseDto> CreateUserRoleAsync(UserRoleRequestDTO createUserRoleDTO, CancellationToken cancellationToken);
        Task<UserRoleResponseDto> GetByIdAsync(Guid urId, CancellationToken cancellationToken);
        Task<UserRoleResponseDto> GetByUserIdAsync(Guid uId, CancellationToken cancellationToken);
        Task<UserRoleResponseDto> UpdateUserRoleAsync(Guid urId, UserRoleRequestDTO updateDto, CancellationToken cancellationToken);
        Task<bool> DeleteUserRoleAsync(Guid urId, CancellationToken cancellationToken);
        Task<PaginationResponse<UserRoleResponseWithExtraDto>> GetAllUserRolesAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
