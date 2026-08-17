using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.CourseTopic
{
    public class CreateMultipleCourseTopicDto
    {
        public int CourseId { get; set; }

        public List<int> TopicIds { get; set; } = new();
    }
}
