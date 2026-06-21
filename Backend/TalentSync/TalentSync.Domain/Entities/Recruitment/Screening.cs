using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class Screening : BaseEntity
    {
        public Guid ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; } = null!;

        public Guid ScreenedById { get; set; }
        public User.User ScreenedBy { get; set; } = null!;

        public ScreeningResult Result { get; set; }

        public string? Notes { get; set; }

        public DateTime ScreeningDate { get; set; }
    }
}
