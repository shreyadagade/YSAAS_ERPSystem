using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.ProgramAnswer
{
    public class UpdateProgramAnswerDto
    {
        public int? ProgramQuestionId { get; set; }

        public string? ProgramAnswer { get; set; }

        public string? ProgramDescription { get; set; }
    }
}
