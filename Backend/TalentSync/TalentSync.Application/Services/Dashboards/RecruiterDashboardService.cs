using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Services.Dashboards
{
    public class RecruiterDashboardService : IRecruiterDashboardService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IMapper _mapper;

        public RecruiterDashboardService(
            IJobRepository jobRepository,
            IApplicationRepository applicationRepository,
            IScreeningRepository screeningRepository,
            IInterviewRepository interviewRepository,
            IMapper mapper)
        {
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _screeningRepository = screeningRepository;
            _interviewRepository = interviewRepository;
            _mapper = mapper;
        }

        public async Task<RecruiterDashboardResponseDto> GetDashboardAsync(Guid uId, CancellationToken cancellationToken)
        {

            int openJobsCount = await _jobRepository.GetOpenJobsCountAsync(cancellationToken);
            int totalApplicationsCount = await _applicationRepository.CountAsync(cancellationToken);
            int pendingScreeningsCount = await _screeningRepository.CountPendingScreeningsAsync(cancellationToken);
            int applicationsTodayCount = await _applicationRepository.CountTodaysApplicationsAsync(cancellationToken);
            int interviewsScheduledCount = await _interviewRepository.GetUpcomingInterviewCountAsync(uId, cancellationToken);
            int closedJobsCount = (await _jobRepository.CountAsync(cancellationToken)) - openJobsCount;
            List<ApplicationEntity> recentApplications = await _applicationRepository.GetRecentApplicationsAsync(5, cancellationToken);
            List<Job> recentJobs = await _jobRepository.GetRecentJobsAsync(5, cancellationToken);
            


            var dashboardResponse = new RecruiterDashboardResponseDto
            {
                OpenJobs = openJobsCount,
                TotalApplications = totalApplicationsCount,
                PendingScreenings = pendingScreeningsCount,
                ApplicationsToday = applicationsTodayCount,
                InterviewsScheduled = interviewsScheduledCount,
                ClosedJobs = closedJobsCount,
                RecentApplications = _mapper.Map<List<ApplicationWithDetailsResponseDto>>(recentApplications),
                RecentJobs = _mapper.Map<List<DashboardJobDto>>(recentJobs)
            };
            
            return dashboardResponse;
        }
    }
}
