using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, RoleName? role);
        string GenerateRefreshToken();
    }
}
