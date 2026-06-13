using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Infrastructure.Persistence.Seeders
{
    public class UserRoleSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IConfiguration configuration)
        {
            string email = configuration["AdminUser:Email"] ?? "";

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Admin email is missing.");
            }

            var adminUser = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (adminUser == null)
            {
                throw new InvalidOperationException(
                    $"Admin user '{email}' not found.");
            }

            var adminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == RoleName.Admin);

            if (adminRole == null)
            {
                throw new InvalidOperationException(
                    "Admin role not found.");
            }

            bool alreadyAssigned = await context.UserRoles.AnyAsync(
                ur => ur.UserId == adminUser.Id &&
                ur.RoleId == adminRole.Id);



            if (alreadyAssigned)
            {
                return;
            }

            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await context.SaveChangesAsync();

        }
    }
}
