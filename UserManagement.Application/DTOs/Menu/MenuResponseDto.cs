namespace UserManagement.Application.DTOs.Menu
{
    public class MenuResponseDto
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; } = string.Empty;

        public string? MenuUrl { get; set; }

        public int? ParentMenuId { get; set; }

        public string? Icon { get; set; }

        public int? DisplayOrder { get; set; }
    }
}