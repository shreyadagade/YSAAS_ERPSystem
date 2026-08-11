using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManagement.Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Employee name is required.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        public string MobileNumber { get; set; }

        public int? BranchId { get; set; }
    }
}
