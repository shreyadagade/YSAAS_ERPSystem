using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingTopic
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public string? PublicFolderId { get; set; }
    }
}
