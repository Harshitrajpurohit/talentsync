using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
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
            return await _context.Jobs.AsNoTracking().CountAsync(j => !j.IsDeleted,cancellationToken);
        }

        public async Task<int> GetOpenJobsCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Jobs.AsNoTracking().CountAsync(j => !j.IsDeleted && j.Status == JobStatus.Open, cancellationToken);
        }

        public async Task<Job> AddAsync(Job job, CancellationToken cancellationToken)
        {
            return (await _context.Jobs.AddAsync(job, cancellationToken)).Entity;
        }

        public async Task<Job?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Jobs
            .Include(j => j.HR)
            .Include(j => j.Applications)
            .FirstOrDefaultAsync(
                j => j.Id == id && !j.IsDeleted,
                cancellationToken);
        }

        public async Task<Job?> GetCandidateJobByIdAsync( Guid id, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    j => j.Id == id &&
                         !j.IsDeleted &&
                         j.Status == JobStatus.Open,
                    cancellationToken);
        }

        public async Task<List<CandidateJobListDto>> GetPagedOpenJobsAsync(PaginationRequest paginationRequest, Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Where(j =>
                    !j.IsDeleted &&
                    j.Status == JobStatus.Open)
                .OrderByDescending(j => j.CreatedAt)
                .Skip(paginationRequest.PageSize *
                      (paginationRequest.PageNumber - 1))
                .Take(paginationRequest.PageSize)
                .Select(j => new CandidateJobListDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Department = j.Department,
                    PostedDate = j.PostedDate,
                    Status = j.Status,

                    HasApplied = j.Applications
                        .Any(a => a.CandidateId == candidateId)
                })
                .ToListAsync(cancellationToken);
        }


        public async Task<List<JobListDto>> GetPagedAllJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Where(j => !j.IsDeleted)
                .OrderByDescending(j => j.PostedDate)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .Select(j => new JobListDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Department = j.Department,
                    Status = j.Status,
                    PostedDate = j.PostedDate,
                    HRId = j.HRId,
                    HrName = j.HR.Name,
                    ApplicationsCount = j.Applications.Count()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<JobSummaryResponseDto> GetJobSummaryAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Applications
                                    .AsNoTracking()
                                    .Where(a => a.JobId == id && !a.IsDeleted)
                                    .GroupBy(_ => 1)
                                    .Select(g => new JobSummaryResponseDto
                                    {
                                        TotalApplications = g.Count(),
                                        Submitted = g.Count(a => a.Status == ApplicationStatus.Submitted),
                                        Screening = g.Count(a => a.Status == ApplicationStatus.Screening),
                                        Interview = g.Count(a => a.Status == ApplicationStatus.InterviewScheduled || a.Status == ApplicationStatus.InterviewCompleted),
                                        Selected = g.Count(a => a.Status == ApplicationStatus.Selected),
                                        Rejected = g.Count(a => a.Status == ApplicationStatus.Rejected)
                                    })
                                    .FirstOrDefaultAsync(cancellationToken)
                                    ?? new JobSummaryResponseDto();

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

        public async Task<List<Job>> GetRecentJobsAsync(int count, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Where(j => !j.IsDeleted)
                .OrderByDescending(j => j.PostedDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }


    }
}
