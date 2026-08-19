using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.ProgramAnswer
{
    public class ProgramAnswerResponseDto
    {
        public int ProgramAnswerId { get; set; }

        public int? ProgramQuestionId { get; set; }

        public string? QuestionTitle { get; set; }

        public string? ProgramAnswer { get; set; }

        public string? ProgramDescription { get; set; }
    }
}
