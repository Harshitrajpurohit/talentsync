using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Extensions
{
    public static class InterviewQueryExtensions
    {
        public static IQueryable<Interview> ApplyFilters(
        this IQueryable<Interview> query,
        InterviewPaginationRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string search = request.Search.Trim();

                query = query.Where(i =>
                    i.Application.Job.Title.Contains(search) ||
                    i.Application.Candidate.Name.Contains(search));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(i =>
                    i.Status == request.Status.Value);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(i =>
                    i.ScheduledAt >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                DateTime toDateExclusive =
                    request.ToDate.Value.Date.AddDays(1);

                query = query.Where(i =>
                    i.ScheduledAt < toDateExclusive);
            }

            return query;
        }
    }
}
