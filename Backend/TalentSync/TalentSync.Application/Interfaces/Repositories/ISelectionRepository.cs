using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface ISelectionRepository
    {
        Task<Selection> AddAsync(Selection selection, CancellationToken cancellationToken);
        Task<Selection?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Selection?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<Selection?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        void Update(Selection selection);
        Task<bool> ExistsByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
