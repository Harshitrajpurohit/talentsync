using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Application.Services.Dashboards
{
    public class HrDashboardService : IHrDashboardService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public HrDashboardService(
            IJobRepository jobRepository,
            IApplicationRepository applicationRepository,
            IInterviewRepository interviewRepository,
            IUserRoleRepository userRoleRepository,
            IMapper mapper)
        {
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<HrDashboardResponseDto> GetDashboardAsync(Guid hrId, CancellationToken cancellationToken)
        {
            HrDashboardResponseDto response = new();

            response.TotalJobs = await _jobRepository.CountAsync(cancellationToken);

            response.OpenJobs = await _jobRepository.GetOpenJobsCountAsync(cancellationToken);

            response.TotalCandidates = await _userRoleRepository.GetCandidateCountAsync(cancellationToken);

            response.TotalApplications = await _applicationRepository.CountAsync(cancellationToken);

            response.InterviewsToday = await _interviewRepository.GetTodayInterviewCountAsync(cancellationToken);


            var jobs = await _jobRepository.GetRecentJobsAsync(5, cancellationToken);

            response.RecentJobs = _mapper.Map<List<DashboardJobDto>>(jobs);


            var applications = await _applicationRepository.GetRecentApplicationsAsync(5, cancellationToken);

            response.RecentApplications = _mapper.Map<List<ApplicationWithDetailsResponseDto>>(applications);


            var interviews = await _interviewRepository.GetUpcomingInterviewsAsync(5, cancellationToken);

            response.UpcomingInterviews = _mapper.Map<List<InterviewDetailedResponseDto>>(interviews);

            return response;
        }
    }
}
