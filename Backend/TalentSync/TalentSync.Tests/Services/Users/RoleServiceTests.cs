using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Services;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;
using TalentSync.Infrastructure.Repositories;

namespace TalentSync.Tests.Services.Users
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RoleService>> _loggerMock;

        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleService>>();

            _roleService = new RoleService(_roleRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllRolesAsync_Should_Get_All_Roles_Successfully()
        {

            List<Role> roles = new List<Role>
            {
                new Role
                {
                    Name = RoleName.Candidate
                }
            };

            _roleRepositoryMock.Setup(x => x.GetAllRolesAsync(It.IsAny<CancellationToken>()));

            await _roleService.GetAllRolesAsync(CancellationToken.None);

            _roleRepositoryMock.Verify(x => x.GetAllRolesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }


        [Fact]
        public async Task GetRoleByIdAsync_Should_Get_Role_Successfully()
        {
            Guid rId = Guid.NewGuid();

            Role role = new Role
            {
                Id = rId,
                Name = RoleName.Candidate
            };

            RoleResponseDto roleResponse = new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
            };

            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync(role);
            _mapperMock.Setup(x => x.Map<RoleResponseDto>(role)).Returns(roleResponse);

            var result = await _roleService.GetRoleByIdAsync(rId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(rId, result.Id);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(role), Times.Once);

        }

        [Fact]
        public async Task GetRoleByIdAsync_Should_Throw_When_Role_NotFound()
        {
            Guid rId = Guid.NewGuid();


            _roleRepositoryMock.Setup(x => x.GetRoleByIdAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.GetRoleByIdAsync(rId, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Equal("Role not found.", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(It.IsAny<Role>()), Times.Never);

        }

        [Fact]
        public async Task GetRoleByRoleNameAsync_Should_Get_Role_Successfully()
        {
            RoleName roleName = RoleName.Candidate;

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = roleName,
            };

            RoleResponseDto roleResponse = new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
            };

            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(roleName, It.IsAny<CancellationToken>())).ReturnsAsync(role);
            _mapperMock.Setup(x => x.Map<RoleResponseDto>(role)).Returns(roleResponse);

            var result = await _roleService.GetRoleByRoleNameAsync(roleName, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(roleName, result.Name);

            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(roleName, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(role), Times.Once);

        }

        [Fact]
        public async Task GetRoleByRoleNameAsync_Should_Throw_When_Role_NotFound()
        {
            RoleName roleName = RoleName.Candidate;

            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(roleName, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.GetRoleByRoleNameAsync(roleName, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Equal("Role not found.", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(roleName, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(It.IsAny<Role>()), Times.Never);

        }

        [Fact]
        public async Task CreateRoleAsync_Should_Create_Role_Successfully()
        {
            CreateRoleDTO createRoleDTO = new CreateRoleDTO
            {
                Name = RoleName.Candidate
            };
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = createRoleDTO.Name,
            };
            RoleResponseDto roleResponse = new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(createRoleDTO.Name, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);
            _mapperMock.Setup(x => x.Map<Role>(createRoleDTO)).Returns(role);
            _roleRepositoryMock.Setup(x => x.AddRoleAsync(role, It.IsAny<CancellationToken>()));
            _roleRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<RoleResponseDto>(role)).Returns(roleResponse);

            var result = await _roleService.CreateRoleAsync(createRoleDTO, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(createRoleDTO.Name, result.Name);

            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(createRoleDTO.Name, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Role>(createRoleDTO), Times.Once);
            _roleRepositoryMock.Verify(x => x.AddRoleAsync(role, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(role), Times.Once);
        }

        [Fact]
        public async Task CreateRoleAsync_Should_Throw_When_CreateRoleDTO_Is_Null()
        {
            CreateRoleDTO? createRoleDTO = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _roleService.CreateRoleAsync(createRoleDTO!, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.Equal("Value cannot be null. (Parameter 'createRoleDTO')", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(It.IsAny<RoleName>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Role>(It.IsAny<CreateRoleDTO>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.AddRoleAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateRoleAsync_Should_Throw_When_Role_Already_Exists()
        {
            CreateRoleDTO createRoleDTO = new CreateRoleDTO
            {
                Name = RoleName.Candidate
            };
            Role existingRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = createRoleDTO.Name,
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(createRoleDTO.Name, It.IsAny<CancellationToken>())).ReturnsAsync(existingRole);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _roleService.CreateRoleAsync(createRoleDTO, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.Equal("Role already exists.", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(createRoleDTO.Name, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Role>(It.IsAny<CreateRoleDTO>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.AddRoleAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateRoleAsync_Should_Update_Role_Successfully()
        {
            Guid rId = Guid.NewGuid();
            CreateRoleDTO updateRoleDTO = new CreateRoleDTO
            {
                Name = RoleName.Candidate
            };
            Role existingRole = new Role
            {
                Id = rId,
                Name = RoleName.Admin
            };
            RoleResponseDto roleResponse = new RoleResponseDto
            {
                Id = existingRole.Id,
                Name = updateRoleDTO.Name,
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync(existingRole);
            _roleRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<RoleResponseDto>(existingRole)).Returns(roleResponse);

            var result = await _roleService.UpdateRoleAsync(rId, updateRoleDTO, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(updateRoleDTO.Name, result.Name);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(existingRole), Times.Once);

        }

        [Fact]
        public async Task UpdateRoleAsync_Should_Throw_When_Role_NotFound()
        {
            Guid rId = Guid.NewGuid();
            CreateRoleDTO updateRoleDTO = new CreateRoleDTO
            {
                Name = RoleName.Candidate
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.UpdateRoleAsync(rId, updateRoleDTO, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.Equal("Role not found.", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<RoleResponseDto>(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRoleAsync_Should_Delete_Role_Successfully()
        {
            Guid rId = Guid.NewGuid();
            Role existingRole = new Role
            {
                Id = rId,
                Name = RoleName.Candidate
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync(existingRole);
            _roleRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _roleService.DeleteRoleAsync(rId, CancellationToken.None);
            Assert.True(result);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.DeleteRole(existingRole), Times.Once);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task DeleteRoleAsync_Should_Throw_When_Role_NotFound()
        {
            Guid rId = Guid.NewGuid();
            _roleRepositoryMock.Setup(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.DeleteRoleAsync(rId, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.Equal("Role not found.", exception.Message);

            _roleRepositoryMock.Verify(x => x.GetRoleByIdForUpdateAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.DeleteRole(It.IsAny<Role>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task RestoreRoleAsync_Should_Restore_Role_Successfully()
        {
            Guid rId = Guid.NewGuid();
            Role existingRole = new Role
            {
                Id = rId,
                Name = RoleName.Candidate
            };
            _roleRepositoryMock.Setup(x => x.GetRoleByIdIncludingDeletedAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync(existingRole);
            _roleRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            var result = await _roleService.RestoreRoleAsync(rId, CancellationToken.None);
            Assert.True(result);
            
            _roleRepositoryMock.Verify(x => x.GetRoleByIdIncludingDeletedAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.RestoreDeletedRole(existingRole), Times.Once);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task RestoreRoleAsync_Should_Throw_When_Role_NotExist()
        {
            Guid rId = Guid.NewGuid();
            _roleRepositoryMock.Setup(x => x.GetRoleByIdIncludingDeletedAsync(rId, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);
            
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.RestoreRoleAsync(rId, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.Equal("Role not found.", exception.Message);
            
            _roleRepositoryMock.Verify(x => x.GetRoleByIdIncludingDeletedAsync(rId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.RestoreDeletedRole(It.IsAny<Role>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


    }
}
