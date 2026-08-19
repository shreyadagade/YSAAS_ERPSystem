using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.ContentInterviewQuestion
{
    public class ContentInterviewQuestionResponseDto
    {
        public int QuestionId { get; set; }

        public int? ContentId { get; set; }

        public string? ContentName { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }
    }
}
