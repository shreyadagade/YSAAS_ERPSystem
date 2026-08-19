using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.ContentQuestion;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingContentQuestionService : ITrainingContentQuestionService
    {
        private readonly IGenericRepository _repository;
        private const string StoredProcedure = "erpsystem.sp_tblcontent_questions";

        public TrainingContentQuestionService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContentQuestionResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<ContentQuestionResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<ContentQuestionResponseDto> GetByIdAsync(int questionId)
        {
            if (questionId <= 0)
            {
                throw new BadRequestException(
                    "Question ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentQuestionResponseDto>(
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
                    $"Question with ID {questionId} was not found.");
            }

            return question;
        }

        public async Task<ContentQuestionResponseDto> CreateAsync(CreateContentQuestionDto dto)
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

            if (string.IsNullOrWhiteSpace(dto.Option1))
            {
                throw new BadRequestException(
                    "Option 1 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option2))
            {
                throw new BadRequestException(
                    "Option 2 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option3))
            {
                throw new BadRequestException(
                    "Option 3 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option4))
            {
                throw new BadRequestException(
                    "Option 4 is required.");
            }

            if (!dto.CorrectOptionNumber.HasValue ||
                dto.CorrectOptionNumber < 1 ||
                dto.CorrectOptionNumber > 4)
            {
                throw new BadRequestException(
                    "Correct option number must be between 1 and 4.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentQuestionResponseDto>(
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
                        Name = "@option1",
                        Value = dto.Option1.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option2",
                        Value = dto.Option2.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option3",
                        Value = dto.Option3.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option4",
                        Value = dto.Option4.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@correct_option_number",
                        Value = dto.CorrectOptionNumber
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new Exception(
                    "Question creation failed.");
            }

            return question;
        }

        public async Task<ContentQuestionResponseDto> UpdateAsync(int questionId,
            UpdateContentQuestionDto dto)
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

            if (string.IsNullOrWhiteSpace(dto.Option1))
            {
                throw new BadRequestException(
                    "Option 1 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option2))
            {
                throw new BadRequestException(
                    "Option 2 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option3))
            {
                throw new BadRequestException(
                    "Option 3 is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Option4))
            {
                throw new BadRequestException(
                    "Option 4 is required.");
            }

            if (!dto.CorrectOptionNumber.HasValue ||
                dto.CorrectOptionNumber < 1 ||
                dto.CorrectOptionNumber > 4)
            {
                throw new BadRequestException(
                    "Correct option number must be between 1 and 4.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ContentQuestionResponseDto>(
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
                        Name = "@option1",
                        Value = dto.Option1.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option2",
                        Value = dto.Option2.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option3",
                        Value = dto.Option3.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@option4",
                        Value = dto.Option4.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@correct_option_number",
                        Value = dto.CorrectOptionNumber
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new NotFoundException(
                    $"Question with ID {questionId} was not found.");
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