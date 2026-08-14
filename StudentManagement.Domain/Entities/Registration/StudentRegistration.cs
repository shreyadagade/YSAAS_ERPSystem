using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities.Registration
{
   

    public class StudentRegistration
    {
        public int RegistrationId { get; set; }

        public int? StudentId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public double? Discount { get; set; }


        public string? CurrentStatus { get; set; }

       

        public int? CourseId { get; set; }
    }
}

