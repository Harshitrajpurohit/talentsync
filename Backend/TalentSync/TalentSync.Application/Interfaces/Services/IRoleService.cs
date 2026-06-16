using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<List<string>> GetAllRolesAsync();
    }
}
