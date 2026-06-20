using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Services.Recruitment
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public JobService(IJobRepository jobRepository, IMapper mapper, IUserRepository userRepository)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<JobResponseDto> CreateJobAsync(CreateJobDto jobDto, Guid HrId, CancellationToken cancellationToken)
        {

            User? user = await _userRepository.GetUserByIdAsync(HrId, cancellationToken);
            if (user == null) {
                throw new InvalidOperationException("User Is Invalid.");
            }
            if(user.Status != Domain.Enums.User.UserStatus.Active)
            {
                throw new InvalidOperationException("User Is Invalid.");
            }
            

            Job job = _mapper.Map<Job>(jobDto);
            job.PostedDate = DateTime.UtcNow;
            job.CreatedAt = DateTime.UtcNow;
            job.Status = JobStatus.Open;
            job.HRId = HrId;

            Job newJob = await _jobRepository.AddAsync(job, cancellationToken);
            await _jobRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<JobResponseDto>(newJob);
        }

        public async Task<List<JobListDto>> GetAllJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            List<Job> jobs = await _jobRepository.GetPagedJobsAsync(paginationRequest, cancellationToken);

            return _mapper.Map<List<JobListDto>>(jobs);
        }

        public async Task<JobResponseDto> GetJobByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Job? job = await _jobRepository.GetJobByIdAsync(id, cancellationToken);
            if (job == null) {
                throw new KeyNotFoundException("Job Not Found.");
            }

            return _mapper.Map<JobResponseDto>(job);
        }

        public async Task<JobResponseDto> UpdateJobAsync(Guid id, Guid userId,UpdateJobRequestDto updateJobRequestDto, CancellationToken cancellationToken)
        {
            Job? job = await _jobRepository.GetJobByIdAsync(id, cancellationToken);
            if (job == null)
            {
                throw new KeyNotFoundException("Job Not Found.");
            }

            if (job.HRId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only update your own jobs.");
            }

            _mapper.Map(updateJobRequestDto, job);
            job.UpdatedAt = DateTime.UtcNow;
            
            _jobRepository.UpdateJob(job);
            await _jobRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<JobResponseDto>(job);

        }

        public async Task<bool> DeleteJobAsync(Guid id,Guid userId, CancellationToken cancellationToken)
        {
            Job? job = await _jobRepository.GetJobByIdAsync(id, cancellationToken);
            if (job == null)
            {
                throw new KeyNotFoundException("Job Not Found.");
            }
            if (job.HRId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only delete your own jobs.");
            }

            _jobRepository.DeleteJob(job);

            await _jobRepository.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
