using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Services;
using TalentSync.Domain.Entities.User;
using TalentSync.Infrastructure.Repositories;

namespace TalentSync.Tests.Services.Users
{
    public class UserRoleServiceTests
    {

        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserRoleService>> _loggerMock;

        private readonly UserRoleService _userRoleService;

        public UserRoleServiceTests()
        {
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserRoleService>>();


            _userRoleService = new UserRoleService(
                _userRoleRepositoryMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _loggerMock.Object
            );

        }


        [Fact]
        public async Task CreateUserRoleAsync_Should_Create_UserRole_Successfully()
        {

             
            var createUserRoleDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            };

            var user = new User
            {
                Id = createUserRoleDto.UserId
            };

            var role = new Role
            {
                Id = createUserRoleDto.RoleId,
            };

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = role.Id
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);
            _mapperMock.Setup(x => x.Map<UserRole>(createUserRoleDto)).Returns(userRole);
            _userRoleRepositoryMock.Setup(x => x.AddAsync(userRole, It.IsAny<CancellationToken>())).ReturnsAsync(userRole);
            _mapperMock.Setup(x => x.Map<UserRoleResponseDto>(userRole)).Returns(new UserRoleResponseDto { Id = userRole.Id });
            

            var result = await _userRoleService.CreateUserRoleAsync(createUserRoleDto, CancellationToken.None);
            

            Assert.NotNull(result);
            Assert.Equal(userRole.Id, result.Id);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.Update(userRole), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.AddAsync(userRole, It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task CreateUserRoleAsync_Should_Update_When_UserRole_Exists()
        {
             
            var createUserRoleDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            };
            var user = new User
            {
                Id = createUserRoleDto.UserId
            };
            var role = new Role
            {
                Id = createUserRoleDto.RoleId,
            };
            var existingUserRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = Guid.NewGuid(),
                IsDeleted = true
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUserRole);
            _mapperMock.Setup(x => x.Map<UserRoleResponseDto>(existingUserRole)).Returns(new UserRoleResponseDto { Id = existingUserRole.Id });


            
            var result = await _userRoleService.CreateUserRoleAsync(createUserRoleDto, CancellationToken.None);
            

            Assert.NotNull(result);
            Assert.Equal(existingUserRole.Id, result.Id);
            Assert.Equal(createUserRoleDto.RoleId, existingUserRole.RoleId);
            Assert.False(existingUserRole.IsDeleted);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.Update(existingUserRole), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task CreateUserRoleAsync_Should_Throw_When_User_NotFound()
        {
             
            var createUserRoleDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
             

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.CreateUserRoleAsync(createUserRoleDto, CancellationToken.None));

            Assert.Equal("User not found", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task CreateUserRoleAsync_Should_Throw_When_Role_NotFound()
        {
             
            var createUserRoleDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            };
            var user = new User
            {
                Id = createUserRoleDto.UserId
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.CreateUserRoleAsync(createUserRoleDto, CancellationToken.None));

            Assert.Equal("Role not found", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(createUserRoleDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(createUserRoleDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_UserRole_When_Found()
        {
             
            var userRoleId = Guid.NewGuid();
            var userRole = new UserRole
            {
                Id = userRoleId,
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };

            UserRoleResponseDto userRoleResponse = new UserRoleResponseDto
            {
                Id = userRole.Id
            };

            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);
            _mapperMock.Setup(x => x.Map<UserRoleResponseDto>(userRole)).Returns(userRoleResponse);

            
            var result = await _userRoleService.GetByIdAsync(userRoleId, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(userRole.Id, result.Id);
            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserRoleResponseDto>(userRole), Times.Once);

        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_When_UserRole_NotFound()
        {
             
            var userRoleId = Guid.NewGuid();
            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);

             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.GetByIdAsync(userRoleId, CancellationToken.None));
            Assert.Equal("UserRole Not Found with id : " + userRoleId, exception.Message);

            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserRoleResponseDto>(It.IsAny<UserRole>()), Times.Never);

        }

        [Fact]
        public async Task GetByUserIdAsync_Should_Return_UserRole_When_Found()
        {
             
            var userId = Guid.NewGuid();

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = Guid.NewGuid()
            };

            UserRoleResponseDto userRoleResponse = new UserRoleResponseDto
            {
                Id = userRole.Id
            };

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);
            _mapperMock.Setup(x => x.Map<UserRoleResponseDto>(userRole)).Returns(userRoleResponse);

            
            var result = await _userRoleService.GetByUserIdAsync(userId, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(userRole.Id, result.Id);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserRoleResponseDto>(userRole), Times.Once);

        }

        [Fact]
        public async Task GetByUserIdAsync_Should_Throw_When_UserRole_NotFound()
        {
             
            var userId = Guid.NewGuid();
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);

             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.GetByUserIdAsync(userId, CancellationToken.None));

            Assert.Equal("UserRole Not Found with UserId : " + userId, exception.Message);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserRoleResponseDto>(It.IsAny<UserRole>()), Times.Never);

        }

        [Fact]
        public async Task UpdateUserRoleAsync_Should_Update_UserRole_Successfully()
        {
             
            var userRoleId = Guid.NewGuid();
            var updateDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            var user = new User
            {
                Id = updateDto.UserId
            };
            var role = new Role
            {
                Id = updateDto.RoleId
            };
            var existingUserRole = new UserRole
            {
                Id = userRoleId,
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUserRole);

            _mapperMock.Setup(x => x.Map<UserRoleResponseDto>(existingUserRole)).Returns(new UserRoleResponseDto { Id = existingUserRole.Id });

            
            var result = await _userRoleService.UpdateUserRoleAsync(userRoleId, updateDto, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(existingUserRole.Id, result.Id);
            Assert.Equal(updateDto.UserId, existingUserRole.UserId);
            Assert.Equal(updateDto.RoleId, existingUserRole.RoleId);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task UpdateUserRoleAsync_Should_Throw_When_User_NotFound()
        {
             
            var userRoleId = Guid.NewGuid();
            var updateDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);


             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.UpdateUserRoleAsync(userRoleId, updateDto, CancellationToken.None));

            Assert.Equal("User not found", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);



        }


        [Fact]
        public async Task UpdateUserRoleAsync_Should_Throw_When_Role_NotFound()
        {
             
            var userRoleId = Guid.NewGuid();
            var updateDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            var user = new User
            {
                Id = updateDto.UserId
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);
             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.UpdateUserRoleAsync(userRoleId, updateDto, CancellationToken.None));
            Assert.Equal("Role not found", exception.Message);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateUserRoleAsync_Should_Throw_When_UserRole_NotFound()
        {
             
            var userRoleId = Guid.NewGuid();
            var updateDto = new UserRoleRequestDTO
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            var user = new User
            {
                Id = updateDto.UserId
            };
            var role = new Role
            {
                Id = updateDto.RoleId
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);

             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.UpdateUserRoleAsync(userRoleId, updateDto, CancellationToken.None));

            Assert.Equal("UserRole Not Found with id : " + userRoleId, exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(updateDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(updateDto.RoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task DeleteUserRoleAsync_Should_Delete_UserRole_Successfully()
        {
             
            var userRoleId = Guid.NewGuid();
            var existingUserRole = new UserRole
            {
                Id = userRoleId,
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid()
            };
            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUserRole);

            
            var result = await _userRoleService.DeleteUserRoleAsync(userRoleId, CancellationToken.None);

            
            Assert.True(result);

            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.Update(existingUserRole), Times.Once);

        }

        [Fact]
        public async Task DeleteUserRoleAsync_Should_Throw_When_UserRole_NotFound()
        {
             
            var userRoleId = Guid.NewGuid();
            _userRoleRepositoryMock.Setup(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);

             
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userRoleService.DeleteUserRoleAsync(userRoleId, CancellationToken.None));

            Assert.Equal("UserRole Not Found with id : " + userRoleId, exception.Message);

            _userRoleRepositoryMock.Verify(x => x.GetByIdAsync(userRoleId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

        }

        [Fact]
        public async Task GetAllUserRolesAsync_Should_Get_UserRoles_Successfully()
        {
             
            int totalCount = 2;
            PaginationRequest paginationRequest = new PaginationRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            var userRoles = new List<UserRoleResponseWithExtraDto>
            {
                new UserRoleResponseWithExtraDto { 
                    Id = Guid.NewGuid(), 
                    UserId = Guid.NewGuid(), 
                    RoleId = Guid.NewGuid() 
                },
                new UserRoleResponseWithExtraDto { 
                    Id = Guid.NewGuid(), 
                    UserId = Guid.NewGuid(), 
                    RoleId = Guid.NewGuid() 
                }
            };
            
            PaginationResponse<UserRoleResponseWithExtraDto> paginationResponse = new PaginationResponse<UserRoleResponseWithExtraDto>
            (
                pageNumber : paginationRequest.PageNumber,
                pageSize : paginationRequest.PageSize,
                totalRecords : totalCount,
                data : userRoles
            );

            _userRoleRepositoryMock.Setup(x => x.CountActiveUserRoleAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);
            _userRoleRepositoryMock.Setup(x => x.GetAllUserRolesAsync(paginationRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRoles);

            
            var result = await _userRoleService.GetAllUserRolesAsync(paginationRequest, CancellationToken.None);
            

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecords);
            _userRoleRepositoryMock.Verify(x => x.GetAllUserRolesAsync(paginationRequest, It.IsAny<CancellationToken>()), Times.Once);
    
        }


    }
}
