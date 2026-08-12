using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;
using TalentSync.Infrastructure.Persistence;
using TalentSync.Infrastructure.Persistence.Extensions;

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
            return await _context.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && !ur.IsDeleted, cancellationToken);
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

        public async Task<List<UserWithRolesDto>> GetAllUserRolesAsync(
            UserPaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            IQueryable<UserRole> query = _context.UserRoles
                .AsNoTracking();

            query = UserQueryExtensions.ApplyFilters(query, paginationRequest);

            return await query
                .OrderByDescending(ur => ur.CreatedAt)
                .Skip(
                    (paginationRequest.PageNumber - 1) *
                    paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .Select(ur => new UserWithRolesDto
                {
                    Id = ur.Id,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    Name = ur.User.Name,
                    Email = ur.User.Email,
                    Phone = ur.User.Phone,
                    Status = ur.User.Status,
                    Role = ur.Role.Name,
                    IsDeleted = ur.User.IsDeleted,
                    CreatedAt = ur.User.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<int> CountUserRoleAsync(
            UserPaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            IQueryable<UserRole> query = _context.UserRoles
                .AsNoTracking();

            query = UserQueryExtensions.ApplyFilters(
                query,
                paginationRequest);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<int> CountActiveUserRoleAsync(CancellationToken cancellationToken)
        {
            return await _context.UserRoles.AsNoTracking().CountAsync(u => !u.IsDeleted, cancellationToken);
        }

        public async Task<int> GetCandidateCountAsync(CancellationToken cancellationToken)
        {
            return await _context.UserRoles
                .AsNoTracking()
                .Where(ur =>
                    !ur.User.IsDeleted &&
                    ur.Role.Name == Domain.Enums.User.RoleName.Candidate)
                .Select(ur => ur.UserId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<List<UserRoleResponseWithExtraDto>> GetAllUserRoleByRoleAsync(RoleName role, CancellationToken cancellationToken)
        {
            return await _context.UserRoles.AsNoTracking()
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .Where(ur => ur.Role.Name == role && !ur.IsDeleted)
                .Select(ur => new UserRoleResponseWithExtraDto
                {
                    Id = ur.Id,
                    UserId = ur.UserId,
                    UserName = ur.User.Name,
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.Name,
                    CreatedAt = ur.CreatedAt,
                    IsDeleted = ur.IsDeleted,
                }).ToListAsync(cancellationToken);
        }
    }
}
