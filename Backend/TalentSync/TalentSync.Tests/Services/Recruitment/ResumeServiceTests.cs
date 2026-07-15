using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Tests.Services.Recruitment
{
    public class ResumeServiceTests
    {
        private readonly Mock<IResumeRepository> _resumeRepositoryMock;
        private readonly Mock<ICloudinaryService> _cloudinaryServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<ResumeService>> _loggerMock;

        private readonly ResumeService _resumeService;

        public ResumeServiceTests()
        {
            _resumeRepositoryMock = new Mock<IResumeRepository>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<ResumeService>>();

            _resumeService = new ResumeService(
                _resumeRepositoryMock.Object,
                _cloudinaryServiceMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task UploadResumeAsync_Should_Upload_Resume_Successfully()
        {
            Guid candidateId = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            CloudinaryUploadResultDto uploadResult = new CloudinaryUploadResultDto
            {
                FileUrl = "url",
                FileName = fileName,
                FileSize = fileSize,
                ContentType = contentType,
                PublicId = publicId
                
            };

            Resume resume = new Resume
            {
                FileUrl = uploadResult.FileUrl,
                FileName = uploadResult.FileName,
                FileSize = uploadResult.FileSize,
                ContentType = uploadResult.ContentType
            };

            ResumeResponseDto resumeResponse = new ResumeResponseDto
            {
                FileUrl = resume.FileUrl,
                FileName = resume.FileName,
                FileSize = resume.FileSize,
                ContentType = resume.ContentType
            };

            _resumeRepositoryMock.Setup(x => x.ExistsByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _cloudinaryServiceMock.Setup(x => x.UploadResumeAsync(stream, fileName, contentType, fileSize)).ReturnsAsync(uploadResult);
            _mapperMock.Setup(x => x.Map<Resume>(uploadResult)).Returns(resume);
            _resumeRepositoryMock.Setup(x => x.AddAsync(resume, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ResumeResponseDto>(resume)).Returns(resumeResponse);

            var result = await _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(resumeResponse.FileName, result.FileName);

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Once);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Once);
            _cloudinaryServiceMock.Verify(x => x.DeleteResumeAsync(publicId), Times.Never);

        }

        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_Stream_IsNull()
        {
            Guid candidateId = Guid.NewGuid();
            Stream? stream = null;
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Never);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_FileSize_More_Than_5MB()
        {
            Guid candidateId = Guid.NewGuid();
            Stream? stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 6 * 1024 * 1024;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            Assert.Equal("File size cannot exceed 5 MB.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Never);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_FileSize_Less_Than_1Bit()
        {
            Guid candidateId = Guid.NewGuid();
            Stream? stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 0;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            Assert.Equal("Invalid file.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Never);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_ContentType_NotSupporded()
        {
            Guid candidateId = Guid.NewGuid();
            Stream? stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/json";
            long fileSize = 1;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            Assert.Equal("Unsupported file type.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Never);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_Resume_Already_Exists()
        {
            Guid candidateId = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            _resumeRepositoryMock.Setup(x => x.ExistsByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            Assert.Equal("Resume already available, chnage the resume", exception.Message);

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Never);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UploadResumeAsync_Should_Throw_When_Resume_Not_Uploaded()
        {
            Guid candidateId = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            CloudinaryUploadResultDto uploadResult = new CloudinaryUploadResultDto
            {
                FileUrl = "url",
                FileName = fileName,
                FileSize = fileSize,
                ContentType = contentType,
                PublicId = publicId

            };

            Resume resume = new Resume
            {
                FileUrl = uploadResult.FileUrl,
                FileName = uploadResult.FileName,
                FileSize = uploadResult.FileSize,
                ContentType = uploadResult.ContentType
            };

            ResumeResponseDto resumeResponse = new ResumeResponseDto
            {
                FileUrl = resume.FileUrl,
                FileName = resume.FileName,
                FileSize = resume.FileSize,
                ContentType = resume.ContentType
            };

            _resumeRepositoryMock.Setup(x => x.ExistsByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _cloudinaryServiceMock.Setup(x => x.UploadResumeAsync(stream, fileName, contentType, fileSize)).ReturnsAsync(uploadResult);
            _mapperMock.Setup(x => x.Map<Resume>(uploadResult)).Returns(resume);
            _resumeRepositoryMock.Setup(x => x.AddAsync(resume, It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();
            _cloudinaryServiceMock.Setup(x => x.DeleteResumeAsync(publicId));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.UploadResumeAsync(candidateId, stream, fileName, contentType, fileSize, CancellationToken.None));

            _resumeRepositoryMock.Verify(x => x.ExistsByCandidateIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Resume>(It.IsAny<CloudinaryUploadResultDto>()), Times.Once);
            _resumeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Resume>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _cloudinaryServiceMock.Verify(x => x.DeleteResumeAsync(publicId), Times.Once);

        }

        [Fact]
        public async Task GetByCandidateIdAsync_Should_Get_Resume_Successfuly()
        {
            Guid candidateId = Guid.NewGuid();

            Resume resume = new Resume
            {
                CandidateId = candidateId,
                FileUrl = "url",
                FileName = "file"
            };

            ResumeWithDetailsResponseDto resumeWithDetails = new ResumeWithDetailsResponseDto
            {
                CandidateId = resume.CandidateId,
                FileName = resume.FileName
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ResumeWithDetailsResponseDto>(resume)).Returns(resumeWithDetails);

            var result = await _resumeService.GetByCandidateIdAsync(candidateId, CancellationToken.None);

            Assert.Equal(candidateId, result.CandidateId);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ResumeWithDetailsResponseDto>(It.IsAny<Resume>()), Times.Once);
        }

        [Fact]
        public async Task GetByCandidateIdAsync_Should_Throw_When_Resume_NotFound()
        {
            Guid candidateId = Guid.NewGuid();
            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync((Resume?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _resumeService.GetByCandidateIdAsync(candidateId, CancellationToken.None));

            Assert.Equal("Candidate Do not have Any resume available.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ResumeWithDetailsResponseDto>(It.IsAny<Resume>()), Times.Never);
        }

        [Fact]
        public async Task ReplaceResumeAsync_Should_Replace_Resume_Successfully()
        {
            Guid id = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            Resume resume = new Resume
            {
                Id = id,
                FileUrl = "url",
                FileName = fileName,
                FileSize = fileSize,
                ContentType = contentType,
                PublicId = publicId
            };

            CloudinaryUploadResultDto uploadResultDto = new CloudinaryUploadResultDto
            {
                FileName = resume.FileName,
                FileSize = resume.FileSize,
                PublicId = "newId",
                FileUrl = "newUrl"
            };

            ResumeResponseDto resumeResponse = new ResumeResponseDto
            {
                PublicId = uploadResultDto.PublicId,
                FileName = uploadResultDto.FileName,
                FileSize = uploadResultDto.FileSize,
                FileUrl = uploadResultDto.FileUrl,
            };


            _resumeRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _cloudinaryServiceMock.Setup(x => x.UploadResumeAsync(stream, fileName, contentType, fileSize)).ReturnsAsync(uploadResultDto);
            _mapperMock.Setup(x => x.Map(uploadResultDto, resume));

            _resumeRepositoryMock.Setup(x => x.Update(resume));
            _cloudinaryServiceMock.Setup(x => x.DeleteResumeAsync(It.IsAny<string>()));
            _mapperMock.Setup(x => x.Map<ResumeResponseDto>(resume)).Returns(resumeResponse);

            var result = await _resumeService.ReplaceResumeAsync(id, stream, fileName, contentType, fileSize, CancellationToken.None);


            Assert.NotNull(result);
            Assert.Equal(resumeResponse.FileName, result.FileName);

            _resumeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(uploadResultDto, resume), Times.Once);
            _resumeRepositoryMock.Verify(x => x.Update(resume), Times.Once);
            _cloudinaryServiceMock.Verify(x => x.DeleteResumeAsync(It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task ReplaceResumeAsync_Should_Throw_When_Resume_NotFound()
        {
            Guid id = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            _resumeRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Resume?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _resumeService.ReplaceResumeAsync(id, stream, fileName, contentType, fileSize, CancellationToken.None));

            Assert.Equal("Resume Not Found.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _resumeRepositoryMock.Verify(x => x.Update(It.IsAny<Resume>()), Times.Never);
            _cloudinaryServiceMock.Verify(x => x.DeleteResumeAsync(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public async Task ReplaceResumeAsync_Should_Throw_When_Resume_Not_Updated()
        {
            Guid id = Guid.NewGuid();
            Stream stream = new MemoryStream();
            string fileName = "File";
            string contentType = "application/pdf";
            long fileSize = 1;
            string publicId = "12324";

            Resume resume = new Resume
            {
                Id = id,
                FileUrl = "url",
                FileName = fileName,
                FileSize = fileSize,
                ContentType = contentType,
                PublicId = publicId
            };

            CloudinaryUploadResultDto uploadResultDto = new CloudinaryUploadResultDto
            {
                FileName = resume.FileName,
                FileSize = resume.FileSize,
                PublicId = "newId",
                FileUrl = "newUrl"
            };

            _resumeRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _cloudinaryServiceMock.Setup(x => x.UploadResumeAsync(stream, fileName, contentType, fileSize)).ReturnsAsync(uploadResultDto);
            _mapperMock.Setup(x => x.Map(uploadResultDto, resume));

            _resumeRepositoryMock.Setup(x => x.Update(resume)).Throws<InvalidOperationException>();
            _cloudinaryServiceMock.Setup(x => x.DeleteResumeAsync(publicId));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _resumeService.ReplaceResumeAsync(id, stream, fileName, contentType, fileSize, CancellationToken.None));

            _resumeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(uploadResultDto, resume), Times.Once);
            _resumeRepositoryMock.Verify(x => x.Update(resume), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _cloudinaryServiceMock.Verify(x => x.DeleteResumeAsync(It.IsAny<string>()), Times.Once);

        }

    }
}
