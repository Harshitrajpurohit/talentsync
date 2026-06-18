using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> CountUsersAsync(CancellationToken cancellationToken);
        Task<int> CountActiveUsersAsync(CancellationToken cancellationToken);
        Task<List<UserWithRolesResponseDto>> GetAllUsersAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<User?> GetUserByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailIncludingDeletedAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetUserByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByPhoneNumberAsync(string phone, CancellationToken cancellationToken);
    }
}
