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
            
            var exception  = await Assert.ThrowsAsync<KeyNotFoundException>(() => _roleService.GetRoleByIdAsync(rId, CancellationToken.None));

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


    }
}
