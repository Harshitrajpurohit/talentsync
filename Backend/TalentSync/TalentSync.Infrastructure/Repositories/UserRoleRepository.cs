using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.User;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<UserRole?> GetByIdAsync(Guid urId, CancellationToken cancellationToken)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Id == urId && !ur.IsDeleted, cancellationToken);
        }

        public async Task<UserRole?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) { 
            return await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && !ur.IsDeleted, cancellationToken);
        }
        public async Task<UserRole?> GetByUserIdWithRoleAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<UserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .OrderByDescending(ur => ur.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserRole> AddAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            await _context.UserRoles.AddAsync(userRole, cancellationToken);
            return userRole;
        }

        public UserRole Update(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            return userRole;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UserRoleResponseWithExtraDto>> GetAllUserRolesAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var query = _context.UserRoles.AsNoTracking();

            return await query
                .Where(ur => !ur.IsDeleted)
                .OrderByDescending(ur => ur.CreatedAt)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .Select(r => new UserRoleResponseWithExtraDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    RoleId = r.RoleId,
                    RoleName = r.Role.Name.ToString(),
                    UserName = r.User.Name,
                    IsDeleted = r.IsDeleted,
                    CreatedAt = r.CreatedAt
                }).ToListAsync(cancellationToken);
        }

        public async Task<int> CountUserRoleAsync(CancellationToken cancellationToken)
        {
            return await _context.UserRoles.AsNoTracking().CountAsync(cancellationToken);
        }
        public async Task<int> CountActiveUserRoleAsync(CancellationToken cancellationToken)
        {
            return await _context.UserRoles.AsNoTracking().CountAsync(u => !u.IsDeleted, cancellationToken);
        }
    }
}
