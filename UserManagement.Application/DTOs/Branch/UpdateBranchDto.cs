using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Branch
{
    public class UpdateBranchDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be greater than 0.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters.")]
        public string BranchName { get; set; } = string.Empty;
    }
}