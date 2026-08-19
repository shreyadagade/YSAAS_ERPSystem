using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.ProgramQuestion
{
    public class ProgramQuestionResponseDto
    {
        public int ProgramQuestionId { get; set; }
        public int? ContentId { get; set; }
        public string? ContentName { get; set; }
        public string? QuestionTitle { get; set; }
        public string? QuestionDescription { get; set; }
    }
}
