using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().CountAsync(cancellationToken);
        }
        public async Task<int> CountNonDeletedUsersAyns(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().CountAsync(u => !u.IsDeleted, cancellationToken);
        }
        public async Task<int> CountByStatusAsync(UserStatus status, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().CountAsync(u => !u.IsDeleted && u.Status==status, cancellationToken);
        }

        public async Task<List<UserWithRolesResponseDto>> GetAllUsersAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsNoTracking();

            return await query
                .Where(u => !u.IsDeleted)
                .OrderByDescending(u => u.CreatedAt)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .Select(u => new UserWithRolesResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Status = u.Status,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name.ToString()).ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
        }


        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.ToLower() && !u.IsDeleted, cancellationToken);
        }

        public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            user.Email = user.Email.Trim().ToLower();
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
        }

        public async Task<User?> GetUserByEmailIncludingDeletedAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
        }
        public async Task<User?> GetUserByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        public async Task<User?> GetUserByIdIncludingDeletedForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phone, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Phone == phone, cancellationToken);
        }

        public async Task<int> CountCandidatesAsync( CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u =>
                    !u.IsDeleted &&
                    u.UserRoles.Any(ur => ur.Role.Name == RoleName.Candidate))
                .CountAsync(cancellationToken);
        }

        public async Task<List<User>> GetCandidatesAsync(
            PaginationRequest paginationRequest,
            Guid roleId,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u =>
                    !u.IsDeleted 
                    && u.UserRoles.Any(ur => ur.RoleId == roleId)
                    )
                .OrderByDescending(u => u.CreatedAt)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
