using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.User
{
    public class ProfileResponseDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? ProfilePhoto { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? JoiningDate { get; set; }
        public double? Salary { get; set; }
        public string? Qualification { get; set; }
        public string? Gender { get; set; }
        public string? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? AadharCardNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? LocalAddress { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
