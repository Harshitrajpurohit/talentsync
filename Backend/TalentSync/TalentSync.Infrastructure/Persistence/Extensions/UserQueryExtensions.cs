using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Infrastructure.Persistence.Extensions
{
    public static class UserQueryExtensions
    {
        public static IQueryable<UserRole> ApplyFilters(this IQueryable<UserRole> query, UserPaginationRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string search = request.Search.Trim();

                query = query.Where(u =>
                    EF.Functions.Like(u.User.Name, $"%{search}%") ||
                    EF.Functions.Like(u.User.Email, $"%{search}%"));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(u =>
                    u.User.Status == request.Status.Value);
            }

            if (request.Role.HasValue)
            {
                query = query.Where(u => 
                    u.Role.Name == request.Role.Value);
            }

            return query;
        }
    }
}
