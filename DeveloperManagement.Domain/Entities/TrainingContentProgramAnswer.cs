namespace DeveloperManagement.Domain.Entities
{
    public class TrainingContentProgramAnswer
    {
        public int ProgramAnswerId { get; set; }

        public int? ProgramQuestionId { get; set; }

        public string? ProgramAnswer { get; set; }

        public string? ProgramDescription { get; set; }
    }
}