using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.DTOs.Registration
    {
        public class StudentQualificationDto
        {
            public int QualificationId { get; set; }

            public int StudentId { get; set; }

            public string? Qualification { get; set; }

            public int? PassingYear { get; set; }

            public string? University { get; set; }

            public string? Medium { get; set; }

            public int? Percentage { get; set; }

         

           
        }
    }

