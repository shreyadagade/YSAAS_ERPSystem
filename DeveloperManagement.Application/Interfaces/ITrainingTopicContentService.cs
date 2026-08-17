using DeveloperManagement.Application.DTOs.TopicContent;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingTopicContentService
    {
        Task<List<TrainingTopicContentResponseDto>> GetAllAsync();

        Task<TrainingTopicContentResponseDto> GetByIdAsync(int contentId);

        Task<List<TrainingTopicContentResponseDto>> CreateAsync(CreateTrainingTopicContentDto dto);

        Task<int> UpdateAsync(int contentId,UpdateTrainingTopicContentDto dto);

        Task<int> DeleteAsync(int contentId);

        Task<int> RestoreAsync(int contentId);

        Task<List<TrainingTopicContentResponseDto>> GetByTopicAsync(int topicId);
    }
}