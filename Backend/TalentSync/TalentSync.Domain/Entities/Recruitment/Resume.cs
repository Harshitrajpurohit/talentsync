using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class Resume : BaseEntity
    {
        public Guid CandidateId { get; set; }

        public User.User Candidate { get; set; }

        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }

        public ResumeStatus Status { get; set; }

    }
}
