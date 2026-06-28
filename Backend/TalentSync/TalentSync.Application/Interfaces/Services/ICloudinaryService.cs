using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResultDto> UploadResumeAsync(
            Stream stream,
            string fileName,
            string contentType,
            long fileSize
            );
        Task DeleteResumeAsync(string publicId);
    }
}
