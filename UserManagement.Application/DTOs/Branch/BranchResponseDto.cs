using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Branch
{
    public class BranchResponseDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}
