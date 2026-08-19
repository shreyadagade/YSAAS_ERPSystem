using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.ContentInterviewQuestion;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingContentInterviewQuestionService
        : ITrainingContentInterviewQuestionService
    {
        private readonly IGenericRepository _repository;

        private const string StoredProcedure = "erpsystem.sp_tblcontent_interview_questions";

        public TrainingContentInterviewQuestionService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContentInterviewQuestionResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<ContentInterviewQuestionResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<ContentInterviewQuestionResponseDto> GetByIdAsync(int questionId)
        {
            if (questionId <= 0)
            {
                throw new BadRequestException(
                    "Question ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentInterviewQuestionResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question_id",
                        Value = questionId
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new NotFoundException(
                    $"Interview question with ID {questionId} was not found.");
            }

            return question;
        }

        public async Task<ContentInterviewQuestionResponseDto> CreateAsync(
            CreateContentInterviewQuestionDto dto)
        {
            if (!dto.ContentId.HasValue ||
                dto.ContentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.Question))
            {
                throw new BadRequestException(
                    "Question is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Answer))
            {
                throw new BadRequestException(
                    "Answer is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentInterviewQuestionResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Insert"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = dto.ContentId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question",
                        Value = dto.Question.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@answer",
                        Value = dto.Answer.Trim()
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new Exception(
                    "Interview question creation failed.");
            }

            return question;
        }

        public async Task<ContentInterviewQuestionResponseDto> UpdateAsync(int questionId,
            UpdateContentInterviewQuestionDto dto)
        {
            if (questionId <= 0)
            {
                throw new BadRequestException(
                    "Question ID must be greater than 0.");
            }

            if (!dto.ContentId.HasValue ||
                dto.ContentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.Question))
            {
                throw new BadRequestException(
                    "Question is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Answer))
            {
                throw new BadRequestException(
                    "Answer is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentInterviewQuestionResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Update"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question_id",
                        Value = questionId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = dto.ContentId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question",
                        Value = dto.Question.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@answer",
                        Value = dto.Answer.Trim()
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new NotFoundException(
                    $"Interview question with ID {questionId} was not found.");
            }

            return question;
        }

        public async Task<int> DeleteAsync(int questionId)
        {
            if (questionId <= 0)
            {
                throw new BadRequestException(
                    "Question ID must be greater than 0.");
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
                    Name = "@question_id",
                    Value = questionId
                });
        }

        public async Task<int> RestoreAsync(int questionId)
        {
            if (questionId <= 0)
            {
                throw new BadRequestException(
                    "Question ID must be greater than 0.");
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
                    Name = "@question_id",
                    Value = questionId
                });
        }
    }
}