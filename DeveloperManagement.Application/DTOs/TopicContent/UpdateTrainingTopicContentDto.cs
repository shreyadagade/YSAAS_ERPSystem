using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.TopicContent
{
    public class UpdateTrainingTopicContentDto
    {
        public string? ContentName { get; set; }

        public int? TopicId { get; set; }

        public string? Slides { get; set; }

        public string? VideoName { get; set; }
    }
}
