namespace StudentManagement.Application.DTOs.Payment
{
    public class StudentPaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int RegistrationId { get; set; }

        public string? StudentName { get; set; }

        public string? CourseName { get; set; }

        public decimal CourseFee { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal RemainingAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public string? PaymentMode { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int IsPaid { get; set; }
    }
}