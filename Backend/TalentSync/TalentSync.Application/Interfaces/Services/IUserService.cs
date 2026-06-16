using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<PaginationResponse<UserWithRolesResponseDto>> GetAllUsersAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken);
        Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserResponseDto> RestoreUserAsync(Guid id, CancellationToken cancellationToken);
        Task<UserResponseDto> ChangeUserStatusAsync(Guid id, UserStatus newStatus, CancellationToken cancellationToken);
    }
}
