using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.Auth
{
    public class RefreshTokenResponseDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
