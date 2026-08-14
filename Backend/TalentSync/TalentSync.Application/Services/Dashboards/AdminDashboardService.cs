using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services.Dashboards
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILogger<AdminDashboardService> _logger;

        public AdminDashboardService(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            ILogger<AdminDashboardService> logger)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _logger = logger;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync(
            Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Fetching admin dashboard.");

            int totalUsers =
                await _userRepository.CountUsersAsync(
                    cancellationToken);

            int activeUsers =
                await _userRepository.CountByStatusAsync(
                    UserStatus.Active,
                    cancellationToken);

            int inactiveUsers =
                await _userRepository.CountByStatusAsync(
                    UserStatus.Inactive,
                    cancellationToken);

            int suspendedUsers =
                await _userRepository.CountByStatusAsync(
                    UserStatus.Suspended,
                    cancellationToken);

            List<AdminRoleCountDto> usersByRole =
                await _userRoleRepository.GetUserCountByRoleAsync(
                    cancellationToken);

            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                SuspendedUsers = suspendedUsers,
                UsersByRole = usersByRole
            };
        }
    }
}
