using AutoMapper;
using Microsoft.Extensions.Logging;
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
    public class ManagerDashboardService : IManagerDashboardService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ManagerDashboardService> _logger;

        public ManagerDashboardService(
            IJobRepository jobRepository,
            IApplicationRepository applicationRepository,
            IScreeningRepository screeningRepository,
            IInterviewRepository interviewRepository,
            IMapper mapper,
            ILogger<ManagerDashboardService> logger)
        {
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ManagerDashboardDto> GetDashboardAsync(
    Guid managerId,
    CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Fetching dashboard for manager {ManagerId}.",
                managerId);

            int openJobs =
                await _jobRepository.GetOpenJobsCountAsync(
                    cancellationToken);

            int totalApplications =
                await _applicationRepository.CountAsync(
                    cancellationToken);

            int interviewsToday =
                await _interviewRepository.GetTodayInterviewCountByInterviewerIdAsync(
                    managerId,
                    cancellationToken);

            int upcomingInterviews =
                await _interviewRepository.GetUpcomingInterviewCountByInterviewerIdAsync(
                    managerId,
                    cancellationToken);

            int completedInterviews =
                await _interviewRepository.GetCompletedInterviewCountByInterviewerIdAsync(
                    managerId,
                    cancellationToken);

            List<Interview> upcomingInterviewEntities =
                await _interviewRepository.GetUpcomingInterviewsByInterviewerIdAsync(
                    managerId,
                    5,
                    cancellationToken);

            List<ApplicationEntity> recentApplicationEntities =
                await _applicationRepository.GetRecentApplicationsAsync(
                    5,
                    cancellationToken);

            List<Job> recentJobEntities =
                await _jobRepository.GetRecentJobsAsync(
                    5,
                    cancellationToken);

            return new ManagerDashboardDto
            {
                OpenJobs = openJobs,
                TotalApplications = totalApplications,
                InterviewsToday = interviewsToday,
                UpcomingInterviews = upcomingInterviews,
                CompletedInterviews = completedInterviews,

                UpcomingInterviewsList =
                    _mapper.Map<List<InterviewDetailedResponseDto>>(
                        upcomingInterviewEntities),

                RecentApplications =
                    _mapper.Map<List<ApplicationWithDetailsResponseDto>>(
                        recentApplicationEntities),

                RecentJobs =
                    _mapper.Map<List<DashboardJobDto>>(
                        recentJobEntities)
            };
        }

    }
}
