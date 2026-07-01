using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Settings;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IOptions<CloudinarySettings> options, ILogger<CloudinaryService> logger)
        {
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.CloudName) ||
                string.IsNullOrWhiteSpace(options.Value.ApiKey) ||
                string.IsNullOrWhiteSpace(options.Value.ApiSecret))
            {
                throw new InvalidOperationException("Cloudinary configuration is missing.");
            }

            var account = new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);

            _cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };

        }

        public async Task<CloudinaryUploadResultDto> UploadResumeAsync(
            Stream stream,
            string fileName,
            string contentType,
            long fileSize
            )
        {
            _logger.LogInformation("Uploading resume to Cloudinary: {FileName}", fileName);
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = "TalentSync/Resumes",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null || result.SecureUrl == null)
            {
                _logger.LogError("Failed to upload resume to Cloudinary: {FileName}", fileName);
                throw new Exception(result?.Error?.Message ?? "Cloudinary upload failed.");
            }

            _logger.LogInformation("Successfully uploaded resume to Cloudinary: {FileName}, URL: {FileUrl}", fileName, result.SecureUrl.AbsoluteUri);
            return new CloudinaryUploadResultDto
            {
                FileUrl = result.SecureUrl.AbsoluteUri,
                FileName = fileName,
                PublicId = result.PublicId,
                ContentType = contentType,
                FileSize = fileSize
            };

        }

        public async Task DeleteResumeAsync(string publicId)
        {
            _logger.LogInformation("Deleting resume from Cloudinary: {PublicId}", publicId);
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                _logger.LogError("Failed to delete resume from Cloudinary: {PublicId}, Error: {ErrorMessage}", publicId, result.Error.Message);
                throw new Exception(result.Error.Message);
            }
            _logger.LogInformation("Successfully deleted resume from Cloudinary: {PublicId}", publicId);
        }
    }
}
