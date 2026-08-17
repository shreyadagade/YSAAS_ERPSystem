using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.DTOs.CourseTopic;
using DeveloperManagement.Application.DTOs.Topic;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services;

public class TrainingCourseTopicService : ITrainingCourseTopicService
{
    private readonly IGenericRepository _repository;
    private const string StoredProcedure = "erpsystem.sp_tbltraining_course_topics";

    public TrainingCourseTopicService(IGenericRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CourseTopicResponseDto>> GetAllAsync()
    {
        return await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetAll"
            });
    }

    public async Task<CourseTopicResponseDto> GetByIdAsync(
        int courseTopicId)
    {
        if (courseTopicId <= 0)
        {
            throw new BadRequestException(
                "Course Topic ID must be greater than 0.");
        }

        var result =
            await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetById"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_topic_id",
                    Value = courseTopicId
                });

        var courseTopic = result.FirstOrDefault();

        if (courseTopic == null)
        {
            throw new NotFoundException(
                $"Course Topic with ID {courseTopicId} was not found.");
        }

        return courseTopic;
    }

    public async Task<List<CourseTopicResponseDto>> GetByCourseAsync(
        int courseId)
    {
        if (courseId <= 0)
        {
            throw new BadRequestException(
                "Course ID must be greater than 0.");
        }

        return await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetByCourse"
            },
            new StoredProcedureParameter
            {
                Name = "@course_id",
                Value = courseId
            });
    }

    public async Task<List<CourseTopicResponseDto>> CreateMultipleAsync(CreateMultipleCourseTopicDto dto)
    {
        if (dto == null)
        {
            throw new BadRequestException(
                "Course topic data is required.");
        }

        if (dto.CourseId <= 0)
        {
            throw new BadRequestException(
                "Course ID must be greater than 0.");
        }

        if (dto.TopicIds == null || dto.TopicIds.Count == 0)
        {
            throw new BadRequestException(
                "At least one topic must be selected.");
        }

        if (dto.TopicIds.Any(x => x <= 0))
        {
            throw new BadRequestException(
                "Topic ID must be greater than 0.");
        }

        var topicIds = dto.TopicIds.Distinct().ToList();

        var courses = await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
            "erpsystem.sp_tbltraining_courses",
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetById"
            },
            new StoredProcedureParameter
            {
                Name = "@course_id",
                Value = dto.CourseId
            });

        if (!courses.Any())
        {
            throw new NotFoundException(
                $"Course with ID {dto.CourseId} was not found.");
        }

        var topics = await _repository.ExecuteQueryAsync<TopicResponseDto>(
            "erpsystem.sp_tbltraining_topics",
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetAll"
            });

        var invalidTopicIds = topicIds
            .Where(id => !topics.Any(t => t.TopicId == id))
            .ToList();

        if (invalidTopicIds.Any())
        {
            throw new NotFoundException(
                $"Topic(s) with ID {string.Join(", ", invalidTopicIds)} were not found.");
        }

        var existingMappings =
            await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetByCourse"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_id",
                    Value = dto.CourseId
                });

        var existingTopicIds = existingMappings
            .Select(x => x.TopicId)
            .ToHashSet();

        var newTopicIds = topicIds
            .Where(id => !existingTopicIds.Contains(id))
            .ToList();

        if (newTopicIds.Count == 0)
        {
            throw new ConflictException(
                "All selected topics are already assigned to this course.");
        }

        foreach (var topicId in newTopicIds)
        {
            await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Insert"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_id",
                    Value = dto.CourseId
                },
                new StoredProcedureParameter
                {
                    Name = "@topic_id",
                    Value = topicId
                });
        }

        return await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetByCourse"
            },
            new StoredProcedureParameter
            {
                Name = "@course_id",
                Value = dto.CourseId
            });
    }

    public async Task<List<CourseTopicResponseDto>> UpdateCourseTopicsAsync(int courseId,
        UpdateCourseTopicsDto dto)
    {
        if (courseId <= 0)
        {
            throw new BadRequestException(
                "Course ID must be greater than 0.");
        }

        if (dto == null)
        {
            throw new BadRequestException(
                "Course topic data is required.");
        }

        if (dto.TopicIds == null)
        {
            throw new BadRequestException(
                "Topic IDs are required.");
        }

        var topicIds = dto.TopicIds.Distinct().ToList();

        var courses =
            await _repository.ExecuteQueryAsync<TrainingCourseResponseDto>(
                "erpsystem.sp_tbltraining_courses",
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetById"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_id",
                    Value = courseId
                });

        if (!courses.Any())
        {
            throw new NotFoundException(
                $"Course with ID {courseId} was not found.");
        }

        if (topicIds.Any())
        {
            var topics = await _repository.ExecuteQueryAsync<TopicResponseDto>(
                    "erpsystem.sp_tbltraining_topics",
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetAll"
                    });

            var invalidTopicIds = topicIds.Where(id => !topics.Any(t => t.TopicId == id))
                .ToList();

            if (invalidTopicIds.Any())
            {
                throw new NotFoundException(
                    $"Topic(s) with ID {string.Join(", ", invalidTopicIds)} were not found.");
            }
        }

        var existingMappings =
            await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetByCourse"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_id",
                    Value = courseId
                });

        var existingTopicIds = existingMappings
            .Select(x => x.TopicId)
            .ToHashSet();

        var selectedTopicIds = topicIds.ToHashSet();

     
        var topicsToDelete = existingMappings.Where(x => !selectedTopicIds.Contains(x.TopicId))
            .ToList();

        foreach (var mapping in topicsToDelete)
        {
            await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Delete"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_topic_id",
                    Value = mapping.CourseTopicId
                });
        }

        var topicsToInsert = topicIds.Where(id => !existingTopicIds.Contains(id))
            .ToList();

        foreach (var topicId in topicsToInsert)
        {
            await _repository.ExecuteNonQueryAsync(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "Insert"
                },
                new StoredProcedureParameter
                {
                    Name = "@course_id",
                    Value = courseId
                },
                new StoredProcedureParameter
                {
                    Name = "@topic_id",
                    Value = topicId
                });
        }

        return await _repository.ExecuteQueryAsync<CourseTopicResponseDto>(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "GetByCourse"
            },
            new StoredProcedureParameter
            {
                Name = "@course_id",
                Value = courseId
            });
    }

    public async Task DeleteAsync(int courseTopicId)
    {
        if (courseTopicId <= 0)
        {
            throw new BadRequestException(
                "Course Topic ID must be greater than 0.");
        }

        await GetByIdAsync(courseTopicId);

        await _repository.ExecuteNonQueryAsync(
            StoredProcedure,
            new StoredProcedureParameter
            {
                Name = "@Type",
                Value = "Delete"
            },
            new StoredProcedureParameter
            {
                Name = "@course_topic_id",
                Value = courseTopicId
            });
    }

    public async Task RestoreAsync(int courseTopicId)
    {
        if (courseTopicId <= 0)
        {
            throw new BadRequestException(
                "Course Topic ID must be greater than 0.");
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
                Name = "@course_topic_id",
                Value = courseTopicId
            });
    }
}