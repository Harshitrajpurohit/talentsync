using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class Job : BaseEntity
    {
        public string Title { get; set; }
        public string Department {  get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public DateTime PostedDate { get; set; }
        public JobStatus Status { get; set; }
        public Guid HRId { get; set; }
        public User.User HR { get; set; } = null!;

        public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    }
}
