using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Recruitment
{
    public class SelectionRepository : ISelectionRepository
    {
        private readonly ApplicationDbContext _context;

        public SelectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Selection> AddAsync(Selection selection, CancellationToken cancellationToken)
        {
            return (await _context.Selections.AddAsync(selection, cancellationToken)).Entity;
        }

        public async Task<Selection?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Selections.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<Selection?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Selections
                .Include(s => s.Application)
                    .ThenInclude(a => a.Job)
                .Include(s => s.Application)
                    .ThenInclude(a => a.Candidate)
                .FirstOrDefaultAsync(
                    s => s.Id == id && !s.IsDeleted,
                    cancellationToken);
        }

        public async Task<Selection?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Selections.AsNoTracking()
                .Include(s => s.Application)
                    .ThenInclude(a => a.Job)
                .Include(s => s.Application)
                    .ThenInclude(a => a.Candidate)
                .FirstOrDefaultAsync(s => s.ApplicationId == applicationId && !s.IsDeleted, cancellationToken);
        }
        public void Update(Selection selection)
        {
            _context.Selections.Update(selection);
        }

        public async Task<bool> ExistsByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            return await _context.Selections
                .AsNoTracking()
                .AnyAsync(
                    s => s.ApplicationId == applicationId && !s.IsDeleted,
                    cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
