namespace UserManagement.Infrastructure.Persistence.Models
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string? ProfilePhoto { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? JoiningDate { get; set; }

        public double? Salary { get; set; }

        public string? Qualification { get; set; }

        public string? Gender { get; set; }

        public int BranchId { get; set; }

        public string? BranchName { get; set; }

        public string? AadharCardNumber { get; set; }

        public string? PanNumber { get; set; }

        public string? LocalAddress { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}