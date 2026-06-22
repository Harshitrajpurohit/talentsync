using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Recruitment
{
    public class ScreeningRepository : IScreeningRepository
    {
        private readonly ApplicationDbContext _context;
        public ScreeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Screening> AddAsync(Screening screening, CancellationToken cancellationToken)
        {
            return (await _context.Screenings.AddAsync(screening, cancellationToken)).Entity;
        }

        public async Task<Screening?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Screenings
                .Include(s => s.Application).ThenInclude(a => a.Candidate)
                .Include(s => s.Application).ThenInclude(a => a.Job)
                .Include(s => s.ScreenedBy)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, cancellationToken);
        }

        public void Update(Screening screening)
        {
            _context.Screenings.Update(screening);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Screenings.AsNoTracking().AnyAsync(s => s.ApplicationId == applicationId && !s.IsDeleted, cancellationToken);
        }

        public async Task<bool> HasPassedScreeningAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Screenings.AsNoTracking()
                .AnyAsync(s => s.ApplicationId == applicationId 
                                && s.Result == ScreeningResult.Pass 
                                && !s.IsDeleted, cancellationToken);

        }

        public async Task<Screening?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Screenings
                .AsNoTracking()
                .Include(s => s.Application).ThenInclude(a => a.Candidate)
                .Include(s => s.Application).ThenInclude(a => a.Job)
                .Include(s => s.ScreenedBy)
                .FirstOrDefaultAsync(
                    s => s.ApplicationId == applicationId && !s.IsDeleted, cancellationToken);
        }

    }
}
