using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Role name is required.")]
        public string RoleName { get; set; } = string.Empty;
    }
}