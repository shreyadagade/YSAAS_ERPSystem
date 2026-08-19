using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.ContentInterviewQuestion
{
    public class UpdateContentInterviewQuestionDto
    {
        public int? ContentId { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }
    }
}
