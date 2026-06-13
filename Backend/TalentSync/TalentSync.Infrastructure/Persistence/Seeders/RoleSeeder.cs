using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Infrastructure.Persistence.Seeders
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if(await context.Roles.AnyAsync())
            {
                return;
            }

            var roles = Enum.GetValues<RoleName>()
                    .Select(role => new Role
                    {
                        Name = role
                    })
                    .ToList();
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

        }
    }
}
