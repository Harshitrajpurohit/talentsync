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
            return await _context.Applications
                .AsNoTracking()
                .CountAsync(a => !a.IsDeleted, cancellationToken);
        }

        public async Task<int> CountAsync(ApplicationPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            IQueryable<ApplicationEntity> query = _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => !a.IsDeleted);

            if (!string.IsNullOrWhiteSpace(paginationRequest.Search))
            {
                string search = paginationRequest.Search.Trim().ToLower();

                query = query.Where(a =>
                    a.Candidate.Name.ToLower().Contains(search) ||
                    a.Job.Title.ToLower().Contains(search) ||
                    a.Candidate.Email.ToLower().Contains(search));
            }

            if (paginationRequest.Status.HasValue)
            {
                query = query.Where(a =>
                    a.Status == paginationRequest.Status.Value);
            }

            if (paginationRequest.JobId.HasValue)
            {
                query = query.Where(a =>
                    a.JobId == paginationRequest.JobId.Value);
            }

            return await query.CountAsync(cancellationToken);
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
            return await _context.Applications.Include(x => x.Job).FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
        }

        public async Task<ApplicationEntity?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Applications
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

        public async Task<List<ApplicationEntity>> GetPagedApplicationsAsync(ApplicationPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            IQueryable<ApplicationEntity> query = _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => !a.IsDeleted);

            if (paginationRequest.JobId.HasValue)
            {
                query = query.Where(a => a.JobId == paginationRequest.JobId.Value);
            }

            if (paginationRequest.Status.HasValue)
            {
                query = query.Where(a => a.Status == paginationRequest.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(paginationRequest.Search))
            {
                string search = paginationRequest.Search.Trim().ToLower();

                query = query.Where(a =>
                    a.Candidate.Name.ToLower().Contains(search) ||
                    a.Job.Title.ToLower().Contains(search) ||
                    a.Candidate.Email.ToLower().Contains(search));
            }

            return await query
                .OrderByDescending(a => a.SubmittedDate)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetByJobIdAsync(Guid jobId,PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Candidate)
                .Where(a => a.JobId == jobId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => a.CandidateId == candidateId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetPagedByCandidateIdAsync(Guid candidateId, PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => a.CandidateId == candidateId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
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

        public async Task<int> GetTotalApplicationsAsync(Guid candidateId,CancellationToken cancellationToken)
        {
            return await _context.Applications
                .CountAsync(a =>
                    a.CandidateId == candidateId &&
                    !a.IsDeleted,
                    cancellationToken);
        }

        public async Task<int> GetActiveApplicationsAsync(Guid candidateId,CancellationToken cancellationToken){
            return await _context.Applications
                .CountAsync(a =>
                    a.CandidateId == candidateId &&
                    !a.IsDeleted &&
                    (
                     a.Status == ApplicationStatus.Screening ||
                     a.Status == ApplicationStatus.InterviewScheduled ||
                     a.Status == ApplicationStatus.InterviewCompleted),
                    cancellationToken);
        }

        public async Task<int> GetSelectedApplicationsAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .CountAsync(a =>
                    a.CandidateId == candidateId &&
                    !a.IsDeleted &&
                    a.Status == ApplicationStatus.Selected,
                    cancellationToken);
        }

        public async Task<List<ApplicationEntity>> GetRecentApplicationsAsync(Guid candidateId, int count, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Where(a =>
                    a.CandidateId == candidateId &&
                    !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }


        public async Task<List<ApplicationEntity>> GetRecentApplicationsAsync(int count, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
