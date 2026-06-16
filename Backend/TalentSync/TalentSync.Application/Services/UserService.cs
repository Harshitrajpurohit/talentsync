using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.User;
using TalentSync.Application.Mappings.UserMappings;
using AutoMapper;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<PaginationResponse<UserWithRolesResponseDto>> GetAllUsersAsync(PaginationRequest paginationRequest,CancellationToken cancellationToken)
        {
            int totalUsers = await _userRepository.CountActiveUsersAsync(cancellationToken);

            List<UserWithRolesResponseDto> users = await _userRepository.GetAllUsersAsync(paginationRequest, cancellationToken);

            return new PaginationResponse<UserWithRolesResponseDto>(
                pageNumber: paginationRequest.PageNumber,
                pageSize: paginationRequest.PageSize,
                totalRecords: totalUsers,
                data: users
            );

        }
        public async Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }

            return _mapper.Map<UserResponseDto>(user);
        }
        public async Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdForUpdateAsync(id, cancellationToken);
            if(user == null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }

            if (!string.IsNullOrWhiteSpace(updateUserDTO.Email) && !string.Equals(
                                                                    user.Email,
                                                                    updateUserDTO.Email,
                                                                    StringComparison.OrdinalIgnoreCase)
                ){
                var existingUser = await _userRepository.GetUserByEmailIncludingDeletedAsync(updateUserDTO.Email, cancellationToken);
                if (existingUser != null && existingUser.Id != id)
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }
            }

            _mapper.Map(updateUserDTO, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserResponseDto>(user);

        }

        public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdForUpdateAsync(id, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<UserResponseDto> RestoreUserAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdIncludingDeletedAsync(id, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }
            if(!user.IsDeleted)
            {
                throw new InvalidOperationException("User is not deleted.");
            }

            user.IsDeleted = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> ChangeUserStatusAsync(Guid id, UserStatus newStatus, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(UserStatus), newStatus))
            {
                throw new ArgumentException("Invalid Status");
            }

            User? user = await _userRepository.GetUserByIdForUpdateAsync(id, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }
            
            user.Status = newStatus;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
