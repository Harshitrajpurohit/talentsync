using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IResumeService
    {
        Task<ResumeResponseDto> UploadResumeAsync(Guid candidateId, Stream stream, string fileName, string contentType, long fileSize, CancellationToken cancellationToken);
        Task<ResumeWithDetailsResponseDto> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken);
        Task<ResumeResponseDto> ReplaceResumeAsync(Guid id, Stream stream, string fileName, string contentType, long fileSize, CancellationToken cancellationToken);
    }
}
