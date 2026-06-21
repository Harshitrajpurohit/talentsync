using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Common.Workflow
{
    public static class ApplicationStatusValidator
    {
        private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> AllowedTransitions = new Dictionary<ApplicationStatus, ApplicationStatus[]>()
        {
            {
                ApplicationStatus.Submitted,
                new[]
                {
                    ApplicationStatus.Screening,
                    ApplicationStatus.Rejected
                }
            },
            {
                ApplicationStatus.Screening,
                new[]
                {
                    ApplicationStatus.InterviewScheduled,
                    ApplicationStatus.Rejected
                }
            },
            {
                ApplicationStatus.InterviewScheduled,
                new[]
                {
                    ApplicationStatus.Selected,
                    ApplicationStatus.Rejected
                }
            },
            {
                ApplicationStatus.Selected,
                Array.Empty<ApplicationStatus>()
            },
            {
                ApplicationStatus.Rejected,
                Array.Empty<ApplicationStatus>()
            }
        };

        public static bool IsValidTransition(ApplicationStatus currentStatus, ApplicationStatus newStatus)
        {
            return AllowedTransitions.TryGetValue(currentStatus, out var allowed) && allowed.Contains(newStatus);

        }
            
    }
}
