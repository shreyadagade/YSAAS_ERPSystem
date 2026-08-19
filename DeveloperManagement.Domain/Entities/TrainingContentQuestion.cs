using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingContentQuestion
    {
        public int QuestionId { get; set; }

        public int? ContentId { get; set; }

        public string? Question { get; set; }

        public string? Option1 { get; set; }

        public string? Option2 { get; set; }

        public string? Option3 { get; set; }

        public string? Option4 { get; set; }

        public int? CorrectOptionNumber { get; set; }
    }
}