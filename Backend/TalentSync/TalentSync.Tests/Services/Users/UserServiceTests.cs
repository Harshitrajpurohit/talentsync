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
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Tests.Services.Users
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Get_Users_Successfully()
        {
            int totalCount = 1;

            PaginationRequest paginationRequest = new PaginationRequest
            {
                PageSize = 10,
                PageNumber = 1,
            };

            List<UserWithRolesResponseDto> users = new List<UserWithRolesResponseDto>
            {
                new UserWithRolesResponseDto
                {
                    Name = "name",
                }
            };

            PaginationResponse<UserWithRolesResponseDto> paginationResponse = new PaginationResponse<UserWithRolesResponseDto>
            (
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                totalCount,
                users
            );

            _userRepositoryMock.Setup(x => x.CountActiveUsersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);
            _userRepositoryMock.Setup(x => x.GetAllUsersAsync(paginationRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync(paginationRequest, It.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.Equal(paginationResponse.PageNumber, result.PageNumber);

            _userRepositoryMock.Verify(x => x.CountActiveUsersAsync(It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetAllUsersAsync(It.IsAny<PaginationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        }


        [Fact]
        public async Task GetUserByIdAsync_Should_Get_User_Successfully()
        {
            Guid id = Guid.NewGuid();

            User user = new User { 
                Name = "Name",
            };
            UserResponseDto userResponse = new UserResponseDto
            {
                Name = user.Name,
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDto>(user)).Returns(userResponse);

            var result = await _userService.GetUserByIdAsync(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Name, result.Name);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(user), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_Throw_When_User_NotFound()
        {
            Guid id = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserByIdAsync(id, CancellationToken.None));

            Assert.Equal("User Not Found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);
        }


        [Fact]
        public async Task UpdateUserAsync_Should_Update_User_Successfully()
        {

            Guid id = Guid.NewGuid();

            UpdateUserDTO updateUser = new UpdateUserDTO
            {
                Email = "email@gmail.com", 
            };

            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "test@gmail.com",
            };

            UserResponseDto userResponse = new UserResponseDto
            {
                Id = id,
                Name = user.Name,
                Email = updateUser.Email,
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(updateUser.Email, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
            _mapperMock.Setup(x => x.Map(updateUser, user));
            _mapperMock.Setup(x => x.Map<UserResponseDto>(user)).Returns(userResponse);

            var result = await _userService.UpdateUserAsync(id, updateUser, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(updateUser.Email, result.Email);

            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(updateUser.Email, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(updateUser, user), Times.Once);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(user), Times.Once);

        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_When_User_NotFound()
        {
            Guid id = Guid.NewGuid();

            UpdateUserDTO updateUser = new UpdateUserDTO
            {
                Email = "email@gmail.com",
            };


            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateUserAsync(id, updateUser, CancellationToken.None));

            Assert.Equal("User Not Found.", exception.Message);


            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(updateUser.Email, It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map(updateUser, It.IsAny<User>()), Times.Never);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_When_Existing_User_Found()
        {
            Guid id = Guid.NewGuid();

            UpdateUserDTO updateUser = new UpdateUserDTO
            {
                Email = "email@gmail.com",
            };

            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
            };

            User existingUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "email@gmail.com",
            };


            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(updateUser.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserAsync(id, updateUser, CancellationToken.None));

            Assert.Equal("A user with this email already exists.", exception.Message);


            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(updateUser.Email, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(updateUser, It.IsAny<User>()), Times.Never);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task DeleteUserAsync_Should_Delete_User_Successfully()
        {
            Guid id = Guid.NewGuid();

            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _userService.DeleteUserAsync(id, CancellationToken.None);

            Assert.True(result);

            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);


        }

        [Fact]
        public async Task DeleteUserAsync_Should_Throw_When_User_NotFound()
        {
            Guid id = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserAsync(id, CancellationToken.None));

            Assert.Equal("User Not Found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task RestoreUserAsync_Should_Restore_User_Successfully()
        {
            Guid id = Guid.NewGuid();
            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
                IsDeleted = true,
            };

            UserResponseDto responseDto = new UserResponseDto
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
                IsDeleted = false,
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDto>(user)).Returns(responseDto);
            var result = await _userService.RestoreUserAsync(id, CancellationToken.None);

            Assert.Equal(id, result.Id);
            Assert.False(result.IsDeleted);

            _userRepositoryMock.Verify(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Once);

        }

        [Fact]
        public async Task RestoreUserAsync_Should_Throw_When_User_NotFound()
        {
            Guid id = Guid.NewGuid();


            _userRepositoryMock.Setup(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.RestoreUserAsync(id, CancellationToken.None));

            Assert.Equal("User Not Found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task RestoreUserAsync_Should_Throw_When_User_NotDeleted()
        {
            Guid id = Guid.NewGuid();
            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
                IsDeleted = false,
            };


            _userRepositoryMock.Setup(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RestoreUserAsync(id, CancellationToken.None));

            Assert.Equal("User is not deleted.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdIncludingDeletedAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task ChangeUserStatusAsync_Should_Change_User_Status_Successfully()
        {
            Guid id = Guid.NewGuid();
            UserStatus newStatus = UserStatus.Active;

            User user = new User
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
                IsDeleted = false,
                Status = UserStatus.Suspended,
            };

            UserResponseDto userResponse = new UserResponseDto
            {
                Id = id,
                Name = "Name",
                Email = "name@gmail.com",
                IsDeleted = false,
                Status = newStatus,
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDto>(user)).Returns(userResponse);

            var result = await _userService.ChangeUserStatusAsync(id, newStatus, CancellationToken.None);

            Assert.Equal(newStatus, result.Status);
            Assert.Equal(newStatus, user.Status);

            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Once);

        }


        [Fact]
        public async Task ChangeUserStatusAsync_Should_Throw_When_User_NotFound()
        {
            Guid id = Guid.NewGuid();
            UserStatus newStatus = UserStatus.Active;

            _userRepositoryMock.Setup(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.ChangeUserStatusAsync(id, newStatus, CancellationToken.None));

            Assert.Equal("User Not Found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdForUpdateAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);

        }

    }
}
