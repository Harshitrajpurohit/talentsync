using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.User
{
    public class UserRegisterRequestDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public string Email { get; set; }
        [Required]
        [MaxLength (500)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
    }
}
