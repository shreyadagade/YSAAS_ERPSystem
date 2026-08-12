using DeveloperManagement.Application.DTOs.Topic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITopicService
    {
        Task<TopicResponseDto> CreateAsync(CreateTopicDto dto);

        Task<TopicResponseDto> GetByIdAsync(int topicId);

        Task<List<TopicResponseDto>> GetAllAsync();

        Task<TopicResponseDto> UpdateAsync(int topicId,UpdateTopicDto dto);

        Task DeleteAsync(int topicId);

        Task RestoreAsync(int topicId);
    }
}
