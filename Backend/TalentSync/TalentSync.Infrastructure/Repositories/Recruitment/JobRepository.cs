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
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _context.Jobs.CountAsync(j => !j.IsDeleted,cancellationToken);
        }

        public async Task<int> GetOpenJobsCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Jobs.CountAsync(j => !j.IsDeleted && j.Status == JobStatus.Open, cancellationToken);
        }

        public async Task<Job> AddAsync(Job job, CancellationToken cancellationToken)
        {
            return (await _context.Jobs.AddAsync(job, cancellationToken)).Entity;
        }

        public async Task<Job?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted, cancellationToken);
        }

        public async Task<List<Job>> GetPagedJobsAsync(PaginationRequest paginationRequest , CancellationToken cancellationToken)
        {
            return await _context.Jobs.AsNoTracking()
                .Where(j => !j.IsDeleted && j.Status == JobStatus.Open)
                .OrderByDescending(j => j.CreatedAt)
                .Skip(paginationRequest.PageSize * (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }

        //public async Task<List<Job>> GetFilteredJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        //{

        //}

        public void UpdateJob(Job job)
        {
            _context.Jobs.Update(job);
        }

        public void DeleteJob(Job job)
        {
           job.IsDeleted = true;
            job.UpdatedAt = DateTime.UtcNow;
            _context.Jobs.Update(job);
        }

        public async Task<List<Job>> GetJobsByHRIdAsync(Guid hrId, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                            .AsNoTracking()
                            .Where(j => j.HRId == hrId && !j.IsDeleted)
                            .OrderByDescending(j => j.CreatedAt)
                            .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
