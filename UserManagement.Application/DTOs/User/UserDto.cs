using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public int BranchId { get; set; }

        public bool IsActive { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
