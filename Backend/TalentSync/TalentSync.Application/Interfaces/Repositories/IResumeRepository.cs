using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IResumeRepository
    {
        Task<Resume> AddAsync(Resume resume, CancellationToken cancellationToken);
        Task<Resume?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Resume?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<Resume?> GetByPublicId(string publicId, CancellationToken cancellationToken);
        Task<Resume?> GetByCandidateId(Guid candidateId, CancellationToken cancellationToken);
        Task<bool> ExistsByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken);
        void Update(Resume resume);
        void Delete(Resume resume);

    }
}
