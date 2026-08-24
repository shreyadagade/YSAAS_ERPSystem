namespace StudentManagement.Domain.Entities.Payment
{
    public class StudentPayment
    {
        public int PaymentId { get; set; }

        public int? RegistrationId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? PaymentAmount { get; set; }

        public string? PaymentMode { get; set; }

        public string? PaymentDescription { get; set; }

        public int? IsPaid { get; set; }

        // =====================================================
        // DISPLAY / CALCULATED FIELDS
        // Returned by Stored Procedure
        // =====================================================

        public string? StudentName { get; set; }

        public string? CourseName { get; set; }

        public double? CourseFee { get; set; }

        public double? TotalPaid { get; set; }

        public double? RemainingAmount { get; set; }
    }
}

