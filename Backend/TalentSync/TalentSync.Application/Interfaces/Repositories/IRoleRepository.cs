using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<List<string>> GetAllRolesAsync();
    }
}
