using System;

namespace StudentManagement.Application.DTOs.Registration
{
    public class StudentPaymentDto
    {
        public int PaymentId { get; set; }

        public int? RegistrationId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? PaymentAmount { get; set; }

        public string? PaymentMode { get; set; }

        public string? PaymentDescription { get; set; }

      

        public int? IsPaid { get; set; }

       
    }
}