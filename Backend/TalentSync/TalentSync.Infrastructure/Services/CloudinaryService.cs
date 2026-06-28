using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {

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
                throw new Exception(result?.Error?.Message ?? "Cloudinary upload failed.");
            }

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
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }
        }
    }
}
