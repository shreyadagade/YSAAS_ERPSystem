using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Menu
{
    public class UpdateMenuDto
    {
        [Required(ErrorMessage = "Menu name is required.")]
        [StringLength(100, ErrorMessage = "Menu name cannot exceed 100 characters.")]
        public string MenuName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Menu URL cannot exceed 200 characters.")]
        public string? MenuUrl { get; set; }

        public int? ParentMenuId { get; set; }

        [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters.")]
        public string? Icon { get; set; }

        public int? DisplayOrder { get; set; }
    }
}