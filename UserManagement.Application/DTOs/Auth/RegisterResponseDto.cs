using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public string Message { get; set; }

        public string UserId { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }
    }
}
