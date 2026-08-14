using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.TopicContent;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingTopicContentService : ITrainingTopicContentService
    {
        private readonly IGenericRepository _repository;
        private const string StoredProcedure = "erpsystem.sp_tbltraining_topic_contents";

        public TrainingTopicContentService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<TrainingTopicContentResponseDto> CreateAsync(CreateTrainingTopicContentDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException(
                    "Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ContentName))
            {
                throw new BadRequestException(
                    "Content name is required.");
            }

            if (dto.ContentName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Content name cannot exceed 100 characters.");
            }

            if (!dto.TopicId.HasValue || dto.TopicId <= 0)
            {
                throw new BadRequestException(
                    "Topic ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.Slides) &&
                string.IsNullOrWhiteSpace(dto.VideoName))
            {
                throw new BadRequestException(
                    "At least one of Slides or Video Name is required.");
            }

            if (!string.IsNullOrWhiteSpace(dto.VideoName) &&
                dto.VideoName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Video name cannot exceed 100 characters.");
            }

            var existingContents =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetByTopic"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@topic_id",
                        Value = dto.TopicId.Value
                    });

            var duplicateContent = existingContents.Any(x =>
                !string.IsNullOrWhiteSpace(x.ContentName) &&
                x.ContentName.Trim().Equals(
                    dto.ContentName.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            if (duplicateContent)
            {
                throw new ConflictException(
                    "Content name already exists for this topic.");
            }

            var result =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Insert"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_name",
                        Value = dto.ContentName.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@topic_id",
                        Value = dto.TopicId.Value
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@slides",
                        Value = dto.Slides
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@video_name",
                        Value = dto.VideoName?.Trim()
                    });

            var content = result.FirstOrDefault();

            if (content == null)
            {
                throw new Exception(
                    "Content could not be created.");
            }

            return content;
        }

        public async Task<List<TrainingTopicContentResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<TrainingTopicContentResponseDto> GetByIdAsync(int contentId)
        {
            if (contentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = contentId
                    });

            var content = result.FirstOrDefault();

            if (content == null)
            {
                throw new NotFoundException(
                    $"Content with ID {contentId} was not found.");
            }

            return content;
        }

        public async Task<List<TrainingTopicContentResponseDto>> GetByTopicAsync(int topicId)
        {
            if (topicId <= 0)
            {
                throw new BadRequestException(
                    "Topic ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetByTopic"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@topic_id",
                        Value = topicId
                    });

            if (result == null || result.Count == 0)
            {
                throw new NotFoundException(
                    $"No contents found for topic ID {topicId}.");
            }

            return result;
        }

        public async Task<int> UpdateAsync(int contentId,UpdateTrainingTopicContentDto dto)
        {
            if (contentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            if (dto == null)
            {
                throw new BadRequestException(
                    "Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ContentName))
            {
                throw new BadRequestException(
                    "Content name is required.");
            }

            if (dto.ContentName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Content name cannot exceed 100 characters.");
            }

            if (!dto.TopicId.HasValue || dto.TopicId <= 0)
            {
                throw new BadRequestException(
                    "Topic ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.Slides) &&
                string.IsNullOrWhiteSpace(dto.VideoName))
            {
                throw new BadRequestException(
                    "At least one of Slides or Video Name is required.");
            }

            if (!string.IsNullOrWhiteSpace(dto.VideoName) &&
                dto.VideoName.Trim().Length > 100)
            {
                throw new BadRequestException(
                    "Video name cannot exceed 100 characters.");
            }

            var existingContent =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = contentId
                    });

            if (existingContent.FirstOrDefault() == null)
            {
                throw new NotFoundException(
                    $"Content with ID {contentId} was not found.");
            }

            var existingContents =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetByTopic"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@topic_id",
                        Value = dto.TopicId.Value
                    });

            var duplicateContent = existingContents.Any(x =>
                x.ContentId != contentId &&
                !string.IsNullOrWhiteSpace(x.ContentName) &&
                x.ContentName.Trim().Equals(
                    dto.ContentName.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            if (duplicateContent)
            {
                throw new ConflictException(
                    "Content name already exists for this topic.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Update"
                },
                new StoredProcedureParameter
                {
                    Name = "@content_id",
                    Value = contentId
                },
                new StoredProcedureParameter
                {
                    Name = "@content_name",
                    Value = dto.ContentName.Trim()
                },
                new StoredProcedureParameter
                {
                    Name = "@topic_id",
                    Value = dto.TopicId.Value
                },
                new StoredProcedureParameter
                {
                    Name = "@slides",
                    Value = dto.Slides
                },
                new StoredProcedureParameter
                {
                    Name = "@video_name",
                    Value = dto.VideoName?.Trim()
                });
        }

        public async Task<int> DeleteAsync(int contentId)
        {
            if (contentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            var existingContent =
                await _repository.ExecuteQueryAsync<TrainingTopicContentResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = contentId
                    });

            if (existingContent.FirstOrDefault() == null)
            {
                throw new NotFoundException(
                    $"Content with ID {contentId} was not found.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Delete"
                },
                new StoredProcedureParameter
                {
                    Name = "@content_id",
                    Value = contentId
                });
        }

        public async Task<int> RestoreAsync(int contentId)
        {
            if (contentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            return await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Restore"
                },
                new StoredProcedureParameter
                {
                    Name = "@content_id",
                    Value = contentId
                });
        }


    }
}