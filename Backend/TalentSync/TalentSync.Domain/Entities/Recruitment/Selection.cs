using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class Selection : BaseEntity
    {
        public Guid ApplicationId { get; set; }

        public ApplicationEntity Application { get; set; } = null!;

        public SelectionDecision Decision { get; set; }

        public string? Notes { get; set; }

        public DateTime SelectionDate { get; set; }

    }
}
