using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Infrastructure.Persistence;
using TalentSync.Infrastructure.Persistence.Extensions;

namespace TalentSync.Infrastructure.Repositories.Recruitment
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Interview> AddAsync(Interview interview, CancellationToken cancellationToken)
        {
            return (await _context.Interviews.AddAsync(interview, cancellationToken)).Entity;
        }

        public async Task<Interview?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Interviews
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, cancellationToken);
        }
        public async Task<Interview?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, cancellationToken);
        }

        public async Task<Interview?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => i.ApplicationId == applicationId && !i.IsDeleted)
                .OrderByDescending(i => i.ScheduledAt)
                .FirstOrDefaultAsync(cancellationToken);
        }


        public void Update(Interview interview)
        {
            _context.Update(interview);
        }

        public async Task<int> CountByInterviewerIdAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            IQueryable<Interview> query = _context.Interviews
                .AsNoTracking()
                .Where(i =>
                    i.InterviewerId == interviewerId &&
                    !i.IsDeleted);

            query = InterviewQueryExtensions.ApplyFilters(query, paginationRequest);

            return await query.CountAsync(cancellationToken);

        }

        public async Task<List<Interview>> GetByInterviewerIdAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var query = _context.Interviews.AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => i.InterviewerId == interviewerId && !i.IsDeleted);
            
                query = InterviewQueryExtensions.ApplyFilters(query, paginationRequest);

                query = query
                    .OrderByDescending(i => i.ScheduledAt)
                    .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize);

            return await query.ToListAsync(cancellationToken); ;
        }

        public async Task<int> GetUpcomingInterviewCountAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Interviews
                .CountAsync(i =>
                    i.Application.CandidateId == candidateId &&
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= DateTime.UtcNow,
                    cancellationToken);
        }

        public async Task<List<Interview>> GetUpcomingInterviewsAsync(Guid candidateId, int count, CancellationToken cancellationToken)
        {
            return await _context.Interviews
                .AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Interviewer)
                .Where(i =>
                    i.Application.CandidateId == candidateId &&
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= DateTime.UtcNow)
                .OrderBy(i => i.ScheduledAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
        public async Task<int> GetTodayInterviewCountAsync(CancellationToken cancellationToken)
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime tomorrow = today.AddDays(1);

            return await _context.Interviews.CountAsync(i =>
                !i.IsDeleted &&
                i.Status == InterviewStatus.Scheduled &&
                i.ScheduledAt >= today &&
                i.ScheduledAt < tomorrow,
                cancellationToken);
        }

        public async Task<List<Interview>> GetUpcomingInterviewsAsync(int count, CancellationToken cancellationToken)
        {
            return await _context.Interviews
                .AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i =>
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= DateTime.UtcNow)
                .OrderBy(i => i.ScheduledAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            IQueryable<Interview> query = _context.Interviews
                .AsNoTracking()
                .Where(i =>
                    i.Application.CandidateId == candidateId &&
                    !i.IsDeleted);

            query = InterviewQueryExtensions.ApplyFilters(query, paginationRequest);

            return await query.CountAsync(cancellationToken);

        }

        public async Task<List<Interview>> GetPagedByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            IQueryable<Interview> query = _context.Interviews
                .AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i =>
                    i.Application.CandidateId == candidateId &&
                    !i.IsDeleted);

            query = InterviewQueryExtensions.ApplyFilters(query, paginationRequest);

            return await query
                .OrderByDescending(i => i.ScheduledAt)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTodayInterviewCountByInterviewerIdAsync(
     Guid interviewerId,
     CancellationToken cancellationToken)
        {
            DateTimeOffset today = DateTimeOffset.UtcNow.Date;
            DateTimeOffset tomorrow = today.AddDays(1);

            return await _context.Interviews
                .AsNoTracking()
                .CountAsync(i =>
                    i.InterviewerId == interviewerId &&
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= today &&
                    i.ScheduledAt < tomorrow,
                    cancellationToken);
        }

        public async Task<int> GetUpcomingInterviewCountByInterviewerIdAsync(
    Guid interviewerId,
    CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            return await _context.Interviews
                .AsNoTracking()
                .CountAsync(i =>
                    i.InterviewerId == interviewerId &&
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= now,
                    cancellationToken);
        }

        public async Task<int> GetCompletedInterviewCountByInterviewerIdAsync(
    Guid interviewerId,
    CancellationToken cancellationToken)
        {
            return await _context.Interviews
                .AsNoTracking()
                .CountAsync(i =>
                    i.InterviewerId == interviewerId &&
                    !i.IsDeleted &&
                    (i.Status == InterviewStatus.Completed || i.Status == InterviewStatus.Passed || i.Status == InterviewStatus.Failed),
                    cancellationToken);
        }

        public async Task<List<Interview>> GetUpcomingInterviewsByInterviewerIdAsync(
    Guid interviewerId,
    int count,
    CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            return await _context.Interviews
                .AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i =>
                    i.InterviewerId == interviewerId &&
                    !i.IsDeleted &&
                    i.Status == InterviewStatus.Scheduled &&
                    i.ScheduledAt >= now)
                .OrderBy(i => i.ScheduledAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

    }
}
