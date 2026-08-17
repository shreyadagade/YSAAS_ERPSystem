using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.CourseTopic
{
    public class UpdateCourseTopicsDto
    {
        public List<int> TopicIds { get; set; } = new();
    }
}
