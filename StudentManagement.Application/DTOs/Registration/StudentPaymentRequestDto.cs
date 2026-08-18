namespace StudentManagement.Application.DTOs.Payment
{
    public class StudentPaymentRequestDto
    {
        public int RegistrationId { get; set; }

        public decimal PaymentAmount { get; set; }

        public string PaymentMode { get; set; }
    }
}