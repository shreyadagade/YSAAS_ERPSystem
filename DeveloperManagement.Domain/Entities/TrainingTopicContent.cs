namespace DeveloperManagement.Domain.Entities
{
    public class TrainingTopicContent
    {
        public int ContentId { get; set; }

        public string? ContentName { get; set; }

        public int? TopicId { get; set; }

        public string? Slides { get; set; }

        public string? VideoName { get; set; }
    }
}