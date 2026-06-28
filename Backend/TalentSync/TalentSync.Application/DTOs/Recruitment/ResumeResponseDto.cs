using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class ResumeResponseDto
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    }
}
