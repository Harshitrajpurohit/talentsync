using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IScreeningRepository
    {
        Task<Screening> AddAsync(Screening screening, CancellationToken cancellationToken);
        Task<Screening?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        void Update(Screening screening);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<bool> ExistsByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        Task<bool> HasPassedScreeningAsync(Guid applicationId, CancellationToken cancellationToken);
        Task<Screening?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
    }
}
