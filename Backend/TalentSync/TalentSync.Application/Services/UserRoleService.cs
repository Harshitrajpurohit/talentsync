using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private IMapper _mapper;
        private readonly ILogger<UserRoleService> _logger;

        public UserRoleService(IUserRoleRepository userRoleRepository, IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, ILogger<UserRoleService> logger)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserRoleResponseDto> CreateUserRoleAsync(UserRoleRequestDTO createUserRoleDTO, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdAsync(createUserRoleDTO.UserId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            Role? role = await _roleRepository.GetRoleByIdAsync(createUserRoleDTO.RoleId, cancellationToken);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdAsync(createUserRoleDTO.UserId, cancellationToken);

            // is user role already available
            if(userRole != null)
            {
                userRole.RoleId = createUserRoleDTO.RoleId;
                userRole.IsDeleted = false;
                userRole.UpdatedAt = DateTime.UtcNow;
                _userRoleRepository.Update(userRole);
                await _userRoleRepository.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("User role updated successfully with ID {UserRole_id}.", userRole.Id);
                return _mapper.Map<UserRoleResponseDto>(userRole);
            }

            // for first user
            userRole = _mapper.Map<UserRole>(createUserRoleDTO);
            userRole.UpdatedAt = DateTime.UtcNow;
            var added = await _userRoleRepository.AddAsync(userRole, cancellationToken);
            await _userRoleRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User role created successfully with ID {UserRole_id}.", added.Id);

            return _mapper.Map<UserRoleResponseDto>(added);
        }
        public async Task<UserRoleResponseDto> GetByIdAsync(Guid urId, CancellationToken cancellationToken)
        {
            UserRole? userRole = await _userRoleRepository.GetByIdAsync(urId, cancellationToken);

            if (userRole == null) {
                _logger.LogWarning("UserRole with ID {UserRole_id} not found.", urId);
                throw new KeyNotFoundException("UserRole Not Found with id : " + urId);
            }
            _logger.LogInformation("UserRole found with ID {UserRole_id}.", userRole.Id);
            return _mapper.Map<UserRoleResponseDto>(userRole);
        }

        public async Task<UserRoleResponseDto> GetByUserIdAsync(Guid uId, CancellationToken cancellationToken)
        {
            UserRole? userRole = await _userRoleRepository.GetByUserIdAsync(uId, cancellationToken);

            if (userRole == null)
            {
                _logger.LogWarning("UserRole with UserId {UserId} not found.", uId);
                throw new KeyNotFoundException("UserRole Not Found with UserId : " + uId);
            }
            _logger.LogInformation("UserRole found with UserId {UserId}.", userRole.UserId);
            return _mapper.Map<UserRoleResponseDto>(userRole);
        }

        public async Task<UserRoleResponseDto> UpdateUserRoleAsync(Guid urId, UserRoleRequestDTO updateDto, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdAsync(updateDto.UserId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            Role? role = await _roleRepository.GetRoleByIdAsync(updateDto.RoleId, cancellationToken);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            UserRole? userRole = await _userRoleRepository.GetByIdAsync(urId, cancellationToken);

            if (userRole == null)
            {
                throw new KeyNotFoundException("UserRole Not Found with id : " + urId);
            }

            if (userRole.RoleId == updateDto.RoleId && userRole.UserId == updateDto.UserId)
            {
                return _mapper.Map<UserRoleResponseDto>(userRole);
            }

            userRole.RoleId = updateDto.RoleId;
            userRole.UserId = updateDto.UserId;
            userRole.UpdatedAt = DateTime.UtcNow;
            _userRoleRepository.Update(userRole);
            await _userRoleRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User role updated successfully with ID {UserRole_id}.", userRole.Id);
            return _mapper.Map<UserRoleResponseDto>(userRole);

        }

        public async Task<bool> DeleteUserRoleAsync(Guid urId, CancellationToken cancellationToken)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(urId, cancellationToken);
            if (userRole == null) {
                throw new KeyNotFoundException("UserRole Not Found with id : " + urId);
            }

            userRole.IsDeleted = true;
            userRole.UpdatedAt = DateTime.UtcNow;
            _userRoleRepository.Update(userRole);
            await _userRoleRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User role deleted successfully with ID {UserRole_id}.", userRole.Id);
            return true;
        }

        public async Task<PaginationResponse<UserRoleResponseWithExtraDto>> GetAllUserRolesAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {

            int totalUserRoles = await _userRoleRepository.CountActiveUserRoleAsync(cancellationToken);

            List<UserRoleResponseWithExtraDto> userRoles = await _userRoleRepository.GetAllUserRolesAsync(paginationRequest, cancellationToken);
            
            _logger.LogInformation("Retrieved all user roles.");
            return new PaginationResponse<UserRoleResponseWithExtraDto>
            (
                pageNumber:paginationRequest.PageNumber,
                pageSize:paginationRequest.PageSize,
                totalRecords:totalUserRoles,
                data:userRoles
            );
        }
    }
}
