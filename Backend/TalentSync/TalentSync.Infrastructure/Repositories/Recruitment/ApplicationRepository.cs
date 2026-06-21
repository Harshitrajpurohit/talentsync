using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Recruitment
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _context.Applications.CountAsync(a => !a.IsDeleted, cancellationToken);
        }


        public async Task<int> CountByJobIdAsync(Guid jobId, CancellationToken cancellationToken)
        {
            return await _context.Applications.CountAsync(a => a.JobId == jobId && !a.IsDeleted,cancellationToken);
        }


        public async Task<ApplicationEntity> AddAsync(ApplicationEntity application, CancellationToken cancellationToken)
        {
            return (await _context.Applications.AddAsync(application, cancellationToken)).Entity;
        }

        public async Task<ApplicationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Applications.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
        }

        public async Task<ApplicationEntity?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Applications.AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
        }



        public async Task<List<ApplicationEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<ApplicationEntity>()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetPagedApplicationsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Applications.AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .Include(a => a.Candidate)
                .Where(a => a.JobId == jobId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Where(a => a.CandidateId == candidateId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> ExistsAsync(Guid jobId, Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .AnyAsync(
                    a => a.JobId == jobId &&
                         a.CandidateId == candidateId &&
                         !a.IsDeleted,
                    cancellationToken);
        }

        public void Update(ApplicationEntity application)
        {
            _context.Applications.Update(application);
        }

        public void Delete(ApplicationEntity application)
        {
            application.IsDeleted = true;

            _context.Applications.Update(application);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
