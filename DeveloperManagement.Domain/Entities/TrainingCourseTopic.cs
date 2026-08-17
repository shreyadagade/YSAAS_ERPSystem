using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingCourseTopic
    {
        public int CourseTopicId { get; set; }
        public int CourseId { get; set; }
        public int TopicId { get; set; }
    }
}
