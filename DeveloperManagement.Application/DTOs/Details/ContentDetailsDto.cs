using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Details
{
    public class ContentDetailsDto
    {
        public int ContentId { get; set; }

        public string ContentName { get; set; }

        public string Slides { get; set; }

        public string VideoName { get; set; }
    }
}
