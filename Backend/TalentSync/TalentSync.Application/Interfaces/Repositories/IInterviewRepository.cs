using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IInterviewRepository
    {
        Task<Interview> AddAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Interview?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Interview>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        void Update(Interview interview);
        Task<List<Interview>> GetByInterviewerIdAsync(Guid interviewerId, CancellationToken cancellationToken);
    }
}
