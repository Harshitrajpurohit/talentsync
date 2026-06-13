using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Infrastructure.Persistence.Seeders
{
    public class UserSeeder
    {
        public static async Task SeedAsync (ApplicationDbContext context, IConfiguration configuration)
        {
            string name = configuration["AdminUser:Name"] ?? "Admin";
            string email = configuration["AdminUser:Email"] ?? "";
            string password = configuration["AdminUser:Password"] ?? "";
            string phone = configuration["AdminUser:Phone"] ?? "";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Admin email or password is missing.");
            }

            if (await context.Users.AnyAsync(u => u.Email == email))
            {
                return;
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Phone = phone,
                Status = UserStatus.Active
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

    }
}
