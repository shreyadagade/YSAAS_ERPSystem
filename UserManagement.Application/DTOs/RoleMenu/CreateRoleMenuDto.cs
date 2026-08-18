using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.RoleMenu
{
    public class CreateRoleMenuDto
    {
        [Required(ErrorMessage = "Role ID is required.")]
        public string RoleId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Menu ID is required.")]
        public int MenuId { get; set; }
    }
}