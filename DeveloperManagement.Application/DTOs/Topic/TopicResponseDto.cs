using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Topic
{
    public class TopicResponseDto
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public string? PublicFolderId { get; set; }
    }
}
