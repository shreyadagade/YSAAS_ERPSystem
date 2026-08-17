using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.TopicContent
{
    public class CreateTrainingTopicContentDto
    {
        public int? TopicId { get; set; }

        public List<ContentItemDto> Contents { get; set; }
            = new List<ContentItemDto>();
    }

    public class ContentItemDto
    {
        public string? ContentName { get; set; }

        public string? Slides { get; set; }

        public string? VideoName { get; set; }
    }
}
