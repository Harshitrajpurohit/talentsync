using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
    }
}
