using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IInterviewRepository
    {
        Task<Interview> AddAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Interview?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<Interview?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        void Update(Interview interview);
        Task<int> CountByInterviewerIdAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<List<Interview>> GetByInterviewerIdAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task<int> GetUpcomingInterviewCountAsync( Guid candidateId, CancellationToken cancellationToken);

        Task<List<Interview>> GetUpcomingInterviewsAsync(Guid candidateId, int count, CancellationToken cancellationToken);
        Task<int> GetTodayInterviewCountAsync(CancellationToken cancellationToken);

        Task<List<Interview>> GetUpcomingInterviewsAsync(int count, CancellationToken cancellationToken);

        Task<int> CountByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task<List<Interview>> GetPagedByCandidateIdAsync( Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task<int> GetTodayInterviewCountByInterviewerIdAsync(
    Guid interviewerId,
    CancellationToken cancellationToken);

        Task<int> GetUpcomingInterviewCountByInterviewerIdAsync(
            Guid interviewerId,
            CancellationToken cancellationToken);

        Task<int> GetCompletedInterviewCountByInterviewerIdAsync(
            Guid interviewerId,
            CancellationToken cancellationToken);

        Task<List<Interview>> GetUpcomingInterviewsByInterviewerIdAsync(
            Guid interviewerId,
            int count,
            CancellationToken cancellationToken);
    }
}
