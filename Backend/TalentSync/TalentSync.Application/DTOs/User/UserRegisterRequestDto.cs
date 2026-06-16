using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.User
{
    public class UserRegisterRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
