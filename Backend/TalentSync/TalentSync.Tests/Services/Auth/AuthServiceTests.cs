using AutoMapper;
using Azure.Core;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Auth;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services;
using TalentSync.Domain.Entities.Auth;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Tests.Services.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;


        private readonly AuthService _service;


        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _loggerMock = new Mock<ILogger<AuthService>>();


            _service = new AuthService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _passwordHasherMock.Object,
                _roleRepositoryMock.Object,
                _userRoleRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _loggerMock.Object

            );
        }

        [Fact]
        public async Task CreateUserAsync_Should_Create_User_Successfully()
        {
            // Arrange

            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            var mappedUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };
            var candidateRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate
            };
            var savedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Status = UserStatus.Active
            };

            var responseDto = new UserResponseDto
            {
                Id = savedUser.Id,
                Name = savedUser.Name,
                Email = savedUser.Email,
                Phone = savedUser.Phone,
                Status = savedUser.Status
            };

            // mock setup
            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegisterRequestDto>())).Returns(mappedUser);

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed-password");

            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(RoleName.Candidate, It.IsAny<CancellationToken>())).ReturnsAsync(candidateRole);

            _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(savedUser);

            _mapperMock.Setup(x => x.Map<UserResponseDto>(It.IsAny<User>())).Returns(responseDto);

            // Act

            var result = await _service.CreateUserAsync(request, CancellationToken.None);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(savedUser.Id, result.Id);
            Assert.Equal(savedUser.Email, result.Email);
            Assert.Equal(savedUser.Name, result.Name);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Email_Already_Exists()
        {

            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };


            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(new User
                                                {
                                                    Email = "harshit@test.com",
                                                    Status = UserStatus.Active,
                                                    IsDeleted = false
                                                });

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));

            Assert.Equal("An account with this email already exists.", exception.Message);


            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Email_DeActive()
        {

            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(new User
                                                {
                                                    Email = "harshit@test.com",
                                                    Status = UserStatus.Inactive,
                                                    IsDeleted = false
                                                });

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));

            Assert.Equal("This email belongs to a deactivated account. Please restore your account.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }



        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Phone_Already_Exists()
        {

            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(new User
                                                {
                                                    Phone = "9876543210",
                                                    Status = UserStatus.Active,
                                                    IsDeleted = false
                                                });
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));

            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            
            _userRepositoryMock.Verify(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Phone_DeActive()
        {

            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            _userRepositoryMock.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(new User
                                                {
                                                    Phone = "9876543210",
                                                    Status = UserStatus.Inactive,
                                                    IsDeleted = false
                                                });
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));

            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);



        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_Candidate_Role_Not_Found()
        {
            // Arrange
            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            var mappedUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            _mapperMock
                .Setup(x => x.Map<User>(It.IsAny<UserRegisterRequestDto>()))
                .Returns(mappedUser);

            _roleRepositoryMock
                .Setup(x => x.GetRoleByRoleNameAsync(RoleName.Candidate, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));

            Assert.Equal("Default Candidate role is not configured.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Throw_When_AddUserAsync_Fails()
        {
            // Arrange
            var request = new UserRegisterRequestDto
            {
                Name = "Harshit",
                Email = "harshit@test.com",
                Password = "Password@123",
                Phone = "9876543210"
            };

            var mappedUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };
            var candidateRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate
            };
            var savedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Status = UserStatus.Active
            };

            _userRepositoryMock.Setup(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegisterRequestDto>())).Returns(mappedUser);

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed-password");

            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(RoleName.Candidate, It.IsAny<CancellationToken>())).ReturnsAsync(candidateRole);

            _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();

            
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(request, CancellationToken.None));


            _userRepositoryMock.Verify(x => x.GetUserByEmailIncludingDeletedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.GetUserByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        }


        [Fact]
        public async Task LogoutAsync_Should_LogoutUser_Successfully()
        {
            // Arrange

            RefreshToken refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                Token = "sample-refresh-token"
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(refreshToken);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            RefreshToken? result = await _service.LogoutAsync("sample-refresh-token", CancellationToken.None);


            Assert.NotNull(result);
            Assert.True(result!.IsRevoked);
            Assert.True(result.UpdatedAt <= DateTime.UtcNow);
            Assert.Equal(refreshToken.Id, result.Id);

            _refreshTokenRepositoryMock.Verify(
                x => x.GetRefreshTokenByTokenAsync(
                    "sample-refresh-token",
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task LogoutAsync_Should_ReturnNull_When_Token_NotFound()
        {
            // Arrange
            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            // Act
            RefreshToken? result = await _service.LogoutAsync("invalid-token", CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task LogoutAsync_Should_NotSave_When_Token_AlreadyRevoked()
        {
            // Arrange
            RefreshToken refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Token = "sample-refresh-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = true
            };

            _refreshTokenRepositoryMock
                .Setup(x => x.GetRefreshTokenByTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            // Act
            RefreshToken? result = await _service.LogoutAsync(
                "sample-refresh-token",
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result!.IsRevoked);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_Should_LoginUser_Successfully()
        {
            // Arrange
            var request = new UserLoginRequestdto
            {
                Email = "harshit@test.com",
                Password = "Password@123"
            };

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Name = "Harshit",
                Email = request.Email,
                PasswordHash = "hashed-password",
                Status = UserStatus.Active
            };

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate
            };

            var userRole = new UserRole
            {
                UserId = userId,
                Role = role
            };

            var accessToken = "jwt-access-token";
            var refreshToken = "refresh-token";


            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(true);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);

            _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user, role.Name))
                .Returns(accessToken);

            _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
                .Returns(refreshToken);


            // Act
            var result = await _service.LoginAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id.ToString(), result.UserId);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(accessToken, result.Token);
            Assert.Equal(refreshToken, result.RefreshToken);
            Assert.Equal(role.Name.ToString(), result.Role);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_User_NotFound()
        {
            // Arrange
            var request = new UserLoginRequestdto
            {
                Email = "harshit@test.com",
                Password = "Password@123"
            };

            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(request, CancellationToken.None));

            Assert.Equal("Invalid email or password.", exception.Message);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _jwtTokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_User_InActive()
        {
            // Arrange
            var request = new UserLoginRequestdto
            {
                Email = "harshit@test.com",
                Password = "Password@123"
            };

            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Harshit",
                    Email = request.Email,
                    PasswordHash = "hashed-password",
                    Status = UserStatus.Inactive
                });

            // Act & Assert

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.LoginAsync(request, CancellationToken.None));
            
            Assert.Equal("Your account is deactivated. Please contact support.", exception.Message);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _jwtTokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_Password_Invalid()
        {
            // Arrange
            var request = new UserLoginRequestdto
            {
                Email = "harshit@test.com",
                Password = "Password@123"
            };


            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Harshit",
                    Email = request.Email,
                    PasswordHash = "hashed-password",
                    Status = UserStatus.Active
                });

            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, "hashed-password"))
                .Returns(false);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(request, CancellationToken.None));

            Assert.Equal("Invalid email or password.", exception.Message);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _jwtTokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_UserRole_NotFound()
        {
            // Arrange
            var request = new UserLoginRequestdto
            {
                Email = "harshit@test.com",
                Password = "Password@123"
            };


            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Harshit",
                    Email = request.Email,
                    PasswordHash = "hashed-password",
                    Status = UserStatus.Active
                });

            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, "hashed-password"))
                .Returns(true);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserRole?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.LoginAsync(request, CancellationToken.None));

            Assert.Equal("No role is assigned to this user.", exception.Message);
            _userRepositoryMock.Verify(x => x.GetUserByEmailAsync(request.Email, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jwtTokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task RefreshTokenAsync_Should_Refresh_Token_Successfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Name = "Harshit",
                Email = "harshit@test.com",
                Status = UserStatus.Active,
                IsDeleted = false
            };

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = "old-refresh-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                User = user
            };

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate
            };

            var userRole = new UserRole
            {
                UserId = userId,
                Role = role
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync( refreshToken.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);

            _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user, role.Name))
                .Returns("new-jwt-token");

            _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
                .Returns("new-refresh-token");

            // Act
            var result = await _service.RefreshTokenAsync(refreshToken.Token, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(role.Name.ToString(), result.Role);
            Assert.Equal("new-jwt-token", result.Token);
            Assert.Equal("new-refresh-token", result.RefreshToken);

            Assert.True(refreshToken.IsRevoked);

            _refreshTokenRepositoryMock.Verify(
                x => x.AddRefreshTokenAsync(
                    It.IsAny<RefreshToken>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_RefreshToken_NotFound()
        {
            // Arrange
            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RefreshTokenAsync("invalid-token", CancellationToken.None));

            Assert.Equal("Invalid refresh token.", exception.Message);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_RefreshToken_Expired()
        {
            // Arrange
            var refreshToken = new RefreshToken
            {
                Token = "expired-token",
                ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
                IsRevoked = false
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync( refreshToken.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RefreshTokenAsync(refreshToken.Token, CancellationToken.None));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_RefreshToken_IsRevoked()
        {
            // Arrange
            var refreshToken = new RefreshToken
            {
                Token = "revoked-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = true
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync( refreshToken.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RefreshTokenAsync(refreshToken.Token, CancellationToken.None));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_User_NotFound()
        {
            // Arrange
            var refreshToken = new RefreshToken
            {
                Token = "refresh-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                User = null
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(refreshToken.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RefreshTokenAsync(refreshToken.Token, CancellationToken.None));

            Assert.Equal("User not found.", exception.Message);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_User_Inactive()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "harshit@test.com",
                Status = UserStatus.Inactive,
                IsDeleted = false
            };

            var refreshToken = new RefreshToken
            {
                Token = "refresh-token",
                UserId = user.Id,
                User = user,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(refreshToken.Token,It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RefreshTokenAsync(refreshToken.Token, CancellationToken.None));

            Assert.Equal("User account is inactive.", exception.Message);

            _userRoleRepositoryMock.Verify(
                x => x.GetByUserIdWithRoleAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Throw_When_UserRole_NotFound()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "harshit@test.com",
                Status = UserStatus.Active,
                IsDeleted = false
            };

            var refreshToken = new RefreshToken
            {
                Token = "refresh-token",
                UserId = user.Id,
                User = user,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByTokenAsync(refreshToken.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserRole?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RefreshTokenAsync(refreshToken.Token,CancellationToken.None));

            Assert.Equal("No role is assigned to this user.", exception.Message);

            _jwtTokenServiceMock.Verify(x => x.GenerateRefreshToken(),
                Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
