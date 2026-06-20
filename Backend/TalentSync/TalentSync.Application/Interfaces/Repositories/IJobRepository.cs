using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> GetOpenJobsCountAsync(CancellationToken cancellationToken);
        Task<Job> AddAsync(Job job, CancellationToken cancellationToken);
        Task<Job?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Job>> GetPagedJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
        Task<List<Job>> GetJobsByHRIdAsync(Guid hrId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
