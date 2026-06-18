using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.Auth
{
    public class UserLoginRequestdto
    {
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public string Email { get; set; }
        [Required]
        [MaxLength (500)]
        public string Password { get; set; }
    }
}
