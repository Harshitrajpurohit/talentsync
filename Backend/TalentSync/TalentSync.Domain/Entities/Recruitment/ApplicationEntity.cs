using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class ApplicationEntity : BaseEntity
    {
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;
        public Guid CandidateId { get; set; }
        public User.User Candidate { get; set; } = null!;
        public DateTime SubmittedDate { get; set; }
        public ApplicationStatus Status { get; set; }

        public ICollection<Screening> Screenings { get; set; } = new List<Screening>();

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();

        public Selection? Selection { get; set; }
    }
}
