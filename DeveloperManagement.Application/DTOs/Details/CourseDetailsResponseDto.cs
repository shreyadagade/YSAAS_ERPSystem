using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Details
{
    public class CourseDetailsResponseDto
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }

        public List<TopicDetailsDto> Topics { get; set; } = new();
    }
}
