using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }
    }
}
