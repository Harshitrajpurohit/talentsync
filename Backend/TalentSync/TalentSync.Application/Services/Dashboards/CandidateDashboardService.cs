using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Services.Dashboards
{
    public class CandidateDashboardService : ICandidateDashboardService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResumeRepository _resumeRepository;
        private readonly IMapper _mapper;

        public CandidateDashboardService(
            IApplicationRepository applicationRepository,
            IInterviewRepository interviewRepository,
            IUserRepository userRepository,
            IResumeRepository resumeRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _userRepository = userRepository;
            _resumeRepository = resumeRepository;
            _mapper = mapper;
        }

        public async Task<CandidateDashboardResponseDto> GetDashboardAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            var totalApplications =
                await _applicationRepository.GetTotalApplicationsAsync( candidateId, cancellationToken);

            var activeApplications =
                await _applicationRepository.GetActiveApplicationsAsync(
                    candidateId,
                    cancellationToken);

            var selectedApplications =
                await _applicationRepository.GetSelectedApplicationsAsync(
                    candidateId,
                    cancellationToken);

            var upcomingInterviewCount =
                await _interviewRepository.GetUpcomingInterviewCountAsync(
                    candidateId,
                    cancellationToken);

            List<ApplicationEntity> recentApplications =
                await _applicationRepository.GetRecentApplicationsAsync(
                    candidateId,
                    5,
                    cancellationToken);


            List<Interview> upcomingInterviews =
                await _interviewRepository.GetUpcomingInterviewsAsync(
                    candidateId,
                    5,
                    cancellationToken);

            var user = await _userRepository.GetUserByIdAsync(
                candidateId,
                cancellationToken);

            var resumeUploaded =
                await _resumeRepository.ExistsByCandidateIdAsync(
                    candidateId,
                    cancellationToken);

            var profileCompletion = CalculateProfileCompletion(user, resumeUploaded);

            return new CandidateDashboardResponseDto
            {
                TotalApplications = totalApplications,
                ActiveApplications = activeApplications,
                SelectedApplications = selectedApplications,
                UpcomingInterviewsCount = upcomingInterviewCount,
                ProfileCompletion = profileCompletion,
                ResumeUploaded = resumeUploaded,
                RecentApplications = _mapper.Map<List<DashboardApplicationDto>>(recentApplications),
                UpcomingInterviews = _mapper.Map<List<DashboardInterviewDto>>(upcomingInterviews)
            };
        }


        private static int CalculateProfileCompletion( User? user, bool resumeUploaded)
        {
            if (user is null)
            {
                throw new KeyNotFoundException("Candidate not found.");
            }

            int completedFields = 0;
            const int totalFields = 7;

            if (!string.IsNullOrWhiteSpace(user.Name))
                completedFields++;

            if (!string.IsNullOrWhiteSpace(user.Phone))
                completedFields++;

            if (!string.IsNullOrWhiteSpace(user.About))
                completedFields++;

            if (!string.IsNullOrWhiteSpace(user.Address))
                completedFields++;

            if (user.DateOfBirth != null)
                completedFields++;

            if (!string.IsNullOrWhiteSpace(user.Gender))
                completedFields++;

            if (resumeUploaded)
                completedFields++;

            return (int)Math.Round((double)completedFields / totalFields * 100);
        }
    }
}
