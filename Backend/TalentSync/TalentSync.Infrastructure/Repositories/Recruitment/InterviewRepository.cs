using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Infrastructure.Persistence;

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

        public async Task<List<Interview>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => i.ApplicationId == applicationId && !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }


        public void Update(Interview interview)
        {
            _context.Update(interview);
        }

        public async Task<List<Interview>> GetByInterviewerIdAsync(Guid interviewerId, CancellationToken cancellationToken)
        {
            return await _context.Interviews.AsNoTracking()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => i.InterviewerId == interviewerId && !i.IsDeleted && i.Status == InterviewStatus.Scheduled)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync(cancellationToken);
        }
    }
}
