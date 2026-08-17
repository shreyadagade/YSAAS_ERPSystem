using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Details
{
    public class CourseDetailsFlatDto
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }

        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public int? ContentId { get; set; }

        public string? ContentName { get; set; }

        public string? Slides { get; set; }

        public string? VideoName { get; set; }
    }
}


