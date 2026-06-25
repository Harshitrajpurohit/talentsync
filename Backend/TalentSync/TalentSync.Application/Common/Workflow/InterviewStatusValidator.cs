using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Common.Workflow
{
    public static class InterviewStatusValidator
    {
        private static readonly Dictionary<InterviewStatus, InterviewStatus[]> AllowedTransitions = new Dictionary<InterviewStatus, InterviewStatus[]>()
        {
            {
                InterviewStatus.Pending,
                new[] { InterviewStatus.Scheduled, InterviewStatus.Cancelled }
            },
            {
                InterviewStatus.Scheduled,
                new[] { InterviewStatus.Completed, InterviewStatus.Cancelled, InterviewStatus.Passed, InterviewStatus.Failed }
            },
            {
                InterviewStatus.Completed,
                new[] { InterviewStatus.Passed, InterviewStatus.Failed }
            },
            {
                InterviewStatus.Passed,
                []
            },
            {
                InterviewStatus.Failed,
                []
            },
            {
                InterviewStatus.Cancelled,
                new[] { InterviewStatus.Scheduled }
            },

        };

        public static bool IsValidTransition(InterviewStatus currStatus, InterviewStatus newStatus)
        {
            return AllowedTransitions.TryGetValue(currStatus, out var allowed) && allowed.Contains(newStatus);
        }
    }
}
