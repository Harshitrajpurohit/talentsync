using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _roleRepository.GetAllRolesAsync(cancellationToken);
        }

        public async Task<RoleResponseDto?> GetRoleByIdAsync(Guid rId, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetRoleByIdAsync(rId, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto?> GetRoleByRoleNameAsync(RoleName name, CancellationToken cancellationToken)
        {
            //if (!Enum.TryParse<RoleName>(name, true, out var roleName))
            //{
            //    throw new ArgumentException("Invalid role name.");
            //}

            Role? role = await _roleRepository.GetRoleByRoleNameAsync(name, cancellationToken);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> CreateRoleAsync(CreateRoleDTO createRoleDTO, CancellationToken cancellationToken)
        {
            if (createRoleDTO == null)
                throw new ArgumentNullException(nameof(createRoleDTO));

            var existingRole = await _roleRepository.GetRoleByRoleNameAsync(createRoleDTO.Name, cancellationToken);

            if (existingRole != null)
                throw new InvalidOperationException("Role already exists.");


            Role role = _mapper.Map<Role>(createRoleDTO);

            await _roleRepository.AddRoleAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(Guid rId, CreateRoleDTO updateRoleDTO, CancellationToken cancellationToken)
        {
            Role? existingRole = await _roleRepository.GetRoleByIdForUpdateAsync(rId, cancellationToken);
            if (existingRole == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }
            existingRole.Name = updateRoleDTO.Name;
            existingRole.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<RoleResponseDto>(existingRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid rId, CancellationToken cancellationToken)
        {
            Role? existingRole = await _roleRepository.GetRoleByIdForUpdateAsync(rId, cancellationToken);
            if (existingRole == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            _roleRepository.DeleteRole(existingRole);
            existingRole.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> RestoreRoleAsync(Guid rId, CancellationToken cancellationToken)
        {
            Role? existingRole = await _roleRepository.GetRoleByIdIncludingDeletedAsync(rId, cancellationToken);
            if (existingRole == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }
            _roleRepository.RestoreDeletedRole(existingRole);
            existingRole.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
