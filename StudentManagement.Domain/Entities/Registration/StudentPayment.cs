using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities.Registration
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

        
    }
}

