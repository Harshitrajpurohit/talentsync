using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Recruitment
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;
        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Resume> AddAsync(Resume resume, CancellationToken cancellationToken)
        {
            return (await _context.Resumes.AddAsync(resume, cancellationToken)).Entity;
        }

        public async Task<Resume?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
        }

        public async Task<Resume?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Resumes
                .AsNoTracking()
                .Include(r => r.Candidate)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
        }

        public async Task<Resume?> GetByPublicId(string publicId, CancellationToken cancellationToken)
        {
            return await _context.Resumes
                .FirstOrDefaultAsync(r => r.PublicId == publicId && !r.IsDeleted, cancellationToken);
        }

        public async Task<Resume?> GetByCandidateId(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Resumes
                .Include(r => r.Candidate)
                .FirstOrDefaultAsync(r => r.CandidateId == candidateId && !r.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            return await _context.Resumes
                .AnyAsync(r =>
                    r.CandidateId == candidateId &&
                    !r.IsDeleted,
                    cancellationToken);
        }

        public void Update(Resume resume)
        {
            _context.Resumes.Update(resume);
        }
        public void Delete(Resume resume)
        {
            resume.IsDeleted = true;
            resume.UpdatedAt = DateTime.UtcNow;

            _context.Resumes.Update(resume);
        }

    }
}
