using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Services.Recruitment
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ResumeService(IResumeRepository resumeRepository, ICloudinaryService cloudinaryService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _resumeRepository = resumeRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResumeResponseDto> UploadResumeAsync(Guid candidateId, Stream stream, string fileName, string contentType, long fileSize, CancellationToken cancellationToken)
        {

            ValidateResume(stream, fileName, contentType, fileSize);

            bool isExists = await _resumeRepository.ExistsByCandidateIdAsync(candidateId, cancellationToken);

            if (isExists)
            {
                throw new InvalidOperationException("Resume already available, chnage the resume");
            }

            CloudinaryUploadResultDto cloudinaryUploadResult = await _cloudinaryService.UploadResumeAsync(stream, fileName, contentType, fileSize);

            try
            {
                Resume resume = _mapper.Map<Resume>(cloudinaryUploadResult);
                resume.CandidateId = candidateId;

                Resume newResume = await _resumeRepository.AddAsync(resume, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _mapper.Map<ResumeResponseDto>(newResume);
            }
            catch
            {
                await _cloudinaryService.DeleteResumeAsync(cloudinaryUploadResult.PublicId);
                throw;
            }
        }


        public async Task<ResumeWithDetailsResponseDto>GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            Resume? resume = await _resumeRepository.GetByCandidateId(candidateId, cancellationToken);
            if(resume == null)
            {
                throw new KeyNotFoundException("Candidate Do not have Any resume available.");
            }

            return _mapper.Map<ResumeWithDetailsResponseDto>(resume);
        }


        public async Task<ResumeResponseDto> ReplaceResumeAsync(Guid id, Stream stream, string fileName, string contentType, long fileSize, CancellationToken cancellationToken)
        {

            ValidateResume(stream, fileName, contentType, fileSize);

            Resume? resume = await _resumeRepository.GetByIdAsync(id, cancellationToken);

            if (resume == null) {
                throw new KeyNotFoundException("Resume Not Found.");
            }

            var oldPublicId = resume.PublicId;
            CloudinaryUploadResultDto cloudinaryUploadResult = await _cloudinaryService.UploadResumeAsync(stream, fileName, contentType, fileSize);
           
            try
            {
                _mapper.Map(cloudinaryUploadResult, resume);
                resume.UploadedDate = DateTime.UtcNow;
                resume.UpdatedAt = DateTime.UtcNow;


                _resumeRepository.Update(resume);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _cloudinaryService.DeleteResumeAsync(oldPublicId);
                return _mapper.Map<ResumeResponseDto>(resume);
            }
            catch
            {
                await _cloudinaryService.DeleteResumeAsync(cloudinaryUploadResult.PublicId);
                throw;
            }

        }


        //File Validation
        private void ValidateResume(Stream stream, string fileName, string contentType, long fileSize) {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            const long maxFileSize = 5 * 1024 * 1024;
            if (fileSize > maxFileSize)
            {
                throw new InvalidOperationException("File size cannot exceed 5 MB.");
            }

            if (fileSize <= 0)
                throw new InvalidOperationException("Invalid file.");


            var allowedContentTypes = new[]
                {
                    "application/pdf",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };

            if (!allowedContentTypes.Contains(contentType))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }
        }
    }
}
