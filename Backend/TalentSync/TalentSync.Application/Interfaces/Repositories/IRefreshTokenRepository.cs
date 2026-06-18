using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Auth;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken);
        Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task RevokeAllUserTokensWithUserIdAsync(Guid UserId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
