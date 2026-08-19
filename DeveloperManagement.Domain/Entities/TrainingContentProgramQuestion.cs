using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingContentProgramQuestion
    {
        public int ProgramQuestionId { get; set; }

        public int? ContentId { get; set; }

        public string? QuestionTitle { get; set; }

        public string? QuestionDescription { get; set; }
    }
}
