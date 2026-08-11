using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Infrastructure.Persistence.Models
{
    public class RegisterEmployeeResult
    {
        public string UserId { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;
    }
}
