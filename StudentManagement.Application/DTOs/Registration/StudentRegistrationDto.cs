using System;

namespace StudentManagement.Application.DTOs.Registration
{
    public class StudentRegistrationDto
    {
        public int RegistrationId { get; set; }

        public int? StudentId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public double? Discount { get; set; }

       

        public string? CurrentStatus { get; set; }

     


        public int? CourseId { get; set; }
    }
}