using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.User
{
    public class CreateRoleDTO
    {
        [Required]
        public RoleName Name { get; set; }
    }
}
