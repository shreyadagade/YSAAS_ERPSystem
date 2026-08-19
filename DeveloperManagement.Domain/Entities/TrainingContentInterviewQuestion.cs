    using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingContentInterviewQuestion
    {
        public int QuestionId { get; set; }

        public int? ContentId { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }
    }
}
