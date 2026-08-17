using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.CourseTopic
{
    public class CourseTopicResponseDto
    {
        public int CourseTopicId { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public int TopicId { get; set; }

        public string TopicName { get; set; }
    }
}
