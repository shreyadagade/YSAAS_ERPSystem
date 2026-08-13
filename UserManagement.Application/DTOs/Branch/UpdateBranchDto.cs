using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Branch
{
    public class UpdateBranchDto
    {
       
        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters.")]
        public string BranchName { get; set; } = string.Empty;
    }
}