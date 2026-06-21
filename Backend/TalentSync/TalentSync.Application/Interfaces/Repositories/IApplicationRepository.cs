using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IApplicationRepository
    {
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountByJobIdAsync(Guid jobId, CancellationToken cancellationToken);
        Task<ApplicationEntity> AddAsync(ApplicationEntity application, CancellationToken cancellationToken);
        Task<ApplicationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<ApplicationEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<ApplicationEntity?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<List<ApplicationEntity>> GetPagedApplicationsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<List<ApplicationEntity>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken);
        Task<List<ApplicationEntity>> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid jobId, Guid candidateId, CancellationToken cancellationToken);
        void Update(ApplicationEntity application);
        void Delete(ApplicationEntity application);
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
