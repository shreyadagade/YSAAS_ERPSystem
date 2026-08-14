using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.Topic;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services;

public class TrainingTopicService : ITrainingTopicService
{
    private readonly IGenericRepository _repository;

    private const string StoredProcedure = "erpsystem.sp_tbltraining_topics";

    public TrainingTopicService(IGenericRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TopicResponseDto>> GetAllAsync()
    {
        return await _repository.ExecuteQueryAsync<TopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetAll"
            });
    }

    public async Task<TopicResponseDto> GetByIdAsync(int topicId)
    {
        if (topicId <= 0)
        {
            throw new BadRequestException(
                "Topic ID must be greater than 0.");
        }

        var result = await _repository.ExecuteQueryAsync<TopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetById"
            },
            new StoredProcedureParameter
            {
                Name = "@topic_id",
                Value = topicId
            });

        var topic = result.FirstOrDefault();

        if (topic == null)
        {
            throw new NotFoundException(
                $"Topic with ID {topicId} was not found.");
        }

        return topic;
    }

    public async Task<TopicResponseDto> CreateAsync(
        CreateTopicDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TopicName))
        {
            throw new BadRequestException(
                "Topic name is required.");
        }

        if (dto.TopicName.Length > 100)
        {
            throw new BadRequestException(
                "Topic name cannot exceed 100 characters.");
        }

        var existingTopics = await _repository.ExecuteQueryAsync<TopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });

        var duplicate = existingTopics.FirstOrDefault(x =>
            x.TopicName.Equals(
                dto.TopicName.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (duplicate != null)
        {
            throw new ConflictException(
                $"Topic '{dto.TopicName}' already exists.");
        }

        await _repository.ExecuteNonQueryAsync(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "Insert"
            },
            new StoredProcedureParameter
            {
                Name = "@topic_name",
                Value = dto.TopicName.Trim()
            },
            new StoredProcedureParameter
            {
                Name = "@publicfolderid",
                Value = dto.PublicFolderId
            });

        var topics = await _repository.ExecuteQueryAsync<TopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });

        var createdTopic = topics
            .FirstOrDefault(x =>
                x.TopicName.Equals(
                    dto.TopicName.Trim(),
                    StringComparison.OrdinalIgnoreCase));

        if (createdTopic == null)
        {
            throw new Exception(
                "Topic was created but could not be retrieved.");
        }

        return createdTopic;
    }

    public async Task<TopicResponseDto> UpdateAsync(int topicId,UpdateTopicDto dto)
    {
        if (topicId <= 0)
        {
            throw new BadRequestException(
                "Topic ID must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(dto.TopicName))
        {
            throw new BadRequestException(
                "Topic name is required.");
        }

        if (dto.TopicName.Length > 100)
        {
            throw new BadRequestException(
                "Topic name cannot exceed 100 characters.");
        }

        var existingTopic = await GetByIdAsync(topicId);

        var allTopics = await _repository.ExecuteQueryAsync<TopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });

        var duplicate = allTopics.FirstOrDefault(x =>
            x.TopicId != topicId &&
            x.TopicName.Equals(
                dto.TopicName.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (duplicate != null)
        {
            throw new ConflictException(
                $"Topic '{dto.TopicName}' already exists.");
        }

        await _repository.ExecuteNonQueryAsync(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "Update"
            },
            new StoredProcedureParameter
            {
                Name = "@topic_id",
                Value = topicId
            },
            new StoredProcedureParameter
            {
                Name = "@topic_name",
                Value = dto.TopicName.Trim()
            },
            new StoredProcedureParameter
            {
                Name = "@publicfolderid",
                Value = dto.PublicFolderId
            });

        return await GetByIdAsync(topicId);
    }

    public async Task DeleteAsync(int topicId)
    {
        if (topicId <= 0)
        {
            throw new BadRequestException(
                "Topic ID must be greater than 0.");
        }

        await GetByIdAsync(topicId);

        await _repository.ExecuteNonQueryAsync(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "Delete"
            },
            new StoredProcedureParameter
            {
                Name = "@topic_id",
                Value = topicId
            });
    }

    public async Task RestoreAsync(int topicId)
    {
        if (topicId <= 0)
        {
            throw new BadRequestException(
                "Topic ID must be greater than 0.");
        }

        await _repository.ExecuteNonQueryAsync(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "Restore"
            },
            new StoredProcedureParameter
            {
                Name = "@topic_id",
                Value = topicId
            });
    }
}