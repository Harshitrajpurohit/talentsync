using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Employees;

namespace TalentSync.Application.DTOs.Employees
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public EmployeeStatus Status { get; set; }
    }
}
