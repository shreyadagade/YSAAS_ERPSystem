using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Account
{
    public class ResetPasswordDto
    {
        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}