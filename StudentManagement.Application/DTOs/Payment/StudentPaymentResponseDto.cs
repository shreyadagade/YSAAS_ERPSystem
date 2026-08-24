namespace StudentManagement.Application.DTOs.Payment
{
    public class StudentPaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int? RegistrationId { get; set; }

        public string? StudentName { get; set; }

        public string? CourseName { get; set; }

        public double? CourseFee { get; set; }

        public double? TotalPaid { get; set; }

        public double? RemainingAmount { get; set; }

        public double? PaymentAmount { get; set; }

        public string? PaymentMode { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? PaymentDescription { get; set; }

        public int? IsPaid { get; set; }
    }
}

