using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Details
{
    public class TopicDetailsDto
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public List<ContentDetailsDto> Contents { get; set; } = new();
    }
}
