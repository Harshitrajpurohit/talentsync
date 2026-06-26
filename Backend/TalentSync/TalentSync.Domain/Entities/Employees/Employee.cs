using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Employees;

namespace TalentSync.Domain.Entities.HumanResources
{
    public class Employee : BaseEntity
    {
        public Guid UserId { get; set; }
        public User.User User { get; set; } = default!;
        public string EmployeeCode { get; set; } = default!;
        public string DepartmentName { get; set; } = default!;
        public string Position { get; set; } = default!;
        public DateTime JoinDate { get; set; }
        public EmployeeStatus Status { get; set; }
        public Guid? ManagerId { get; set; }
        public User.User? Manager { get; set; }
    }
}
