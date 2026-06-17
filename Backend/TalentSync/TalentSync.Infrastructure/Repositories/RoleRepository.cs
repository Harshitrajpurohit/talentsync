using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _context.Roles.AsNoTracking()
                .Where(r => !r.IsDeleted)
                .Select(r => r.Name.ToString())
                .ToListAsync(cancellationToken);

            return roles;
        }

        public async Task<Role?> GetRoleByIdAsync(Guid rId, CancellationToken cancellationToken)
        {
            return await _context.Roles
                    .AsNoTracking()
                    .Where(r => r.Id == rId && !r.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Role?> GetRoleByRoleNameAsync(RoleName roleName, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == roleName && !r.IsDeleted, cancellationToken);
            return role;
        }


        public async Task<Role?> GetRoleByIdForUpdateAsync(Guid rId, CancellationToken cancellationToken)
        {
            return await _context.Roles
                    .Where(r => r.Id == rId && !r.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Role?> GetRoleByRoleNameForUpdateAsync(RoleName roleName, CancellationToken cancellationToken)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName && !r.IsDeleted, cancellationToken);
            return role;
        }

        public async Task<Role> AddRoleAsync(Role role, CancellationToken cancellationToken) { 
            await _context.Roles.AddAsync(role, cancellationToken);
            return role;
        }

        public void DeleteRole(Role role)
        {
            role.IsDeleted = true;
        }

        // Get Deleted Role Also
        public async Task<Role?> GetRoleByIdIncludingDeletedAsync(Guid rId, CancellationToken cancellationToken)
        {
            return await _context.Roles
                    .Where(r => r.Id == rId)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        public void RestoreDeletedRole(Role role)
        {
            role.IsDeleted = false;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
