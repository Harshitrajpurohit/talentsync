using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
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

        public async Task<List<string>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.AsNoTracking()
                .Select(r => r.Name)
                .ToListAsync();

            return roles.Select(r => r.ToString()).ToList();
        }
    }
}
