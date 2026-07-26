using System;
using System.ComponentModel.DataAnnotations;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public UserStatus? Status { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? About { get; set; }

        [Url]
        public string? LinkedinUrl { get; set; }

        [Url]
        public string? GithubUrl { get; set; }

        [Url]
        public string? PortfolioUrl { get; set; }
    }
}