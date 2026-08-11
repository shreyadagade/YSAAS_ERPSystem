using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}
