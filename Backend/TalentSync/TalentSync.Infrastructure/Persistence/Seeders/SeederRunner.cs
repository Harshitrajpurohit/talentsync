using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Infrastructure.Persistence.Seeders
{
    public class SeederRunner
    {
        public static async Task SeedAsync(ApplicationDbContext context, IConfiguration configuration)
        {
            await RoleSeeder.SeedAsync(context);
            await UserSeeder.SeedAsync(context, configuration);
            await UserRoleSeeder.SeedAsync(context, configuration);
        }

    }
}
