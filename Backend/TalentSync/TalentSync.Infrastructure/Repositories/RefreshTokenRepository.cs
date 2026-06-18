using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Auth;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken) {
            return await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsDeleted, cancellationToken);
        }

        public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            return refreshToken;
        }

        public async Task RevokeAllUserTokensWithUserIdAsync(Guid UserId, CancellationToken cancellationToken)
        {
            List<RefreshToken> refreshTokens = await _context.RefreshTokens.Where(rt => rt.UserId == UserId && !rt.IsDeleted && !rt.IsRevoked).ToListAsync(cancellationToken);

            foreach (RefreshToken refreshToken in refreshTokens)
            {
                refreshToken.IsRevoked = true;
            }

        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
