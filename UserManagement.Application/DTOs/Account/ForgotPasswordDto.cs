using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
    }
}