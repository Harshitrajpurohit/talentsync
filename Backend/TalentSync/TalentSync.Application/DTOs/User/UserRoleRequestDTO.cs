using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.User
{
    public class UserRoleRequestDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}
