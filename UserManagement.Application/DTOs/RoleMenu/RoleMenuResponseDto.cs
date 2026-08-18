namespace UserManagement.Application.DTOs.RoleMenu
{
    public class RoleMenuResponseDto
    {
        public int RoleMenuId { get; set; }

        public string RoleId { get; set; } = string.Empty;

        public int MenuId { get; set; }

        public string MenuName { get; set; } = string.Empty;

        public string? MenuUrl { get; set; }

        public int? ParentMenuId { get; set; }

        public string? Icon { get; set; }

        public int? DisplayOrder { get; set; }
    }
}