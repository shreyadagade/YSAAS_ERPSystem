using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Employee name is required.")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNumber { get; set; } = string.Empty;

        public int? BranchId { get; set; }
    }
}