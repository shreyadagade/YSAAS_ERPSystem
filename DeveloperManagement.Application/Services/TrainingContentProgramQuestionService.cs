using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.ProgramQuestion;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingContentProgramQuestionService: ITrainingContentProgramQuestionService
    {
        private readonly IGenericRepository _repository;

        private const string StoredProcedure = "erpsystem.sp_tblcontent_program_questions";

        public TrainingContentProgramQuestionService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProgramQuestionResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<ProgramQuestionResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<ProgramQuestionResponseDto> GetByIdAsync(int programQuestionId)
        {
            if (programQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramQuestionResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_question_id",
                        Value = programQuestionId
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new NotFoundException(
                    $"Program question with ID {programQuestionId} was not found.");
            }

            return question;
        }

        public async Task<ProgramQuestionResponseDto> CreateAsync(CreateProgramQuestionDto dto)
        {
            if (!dto.ContentId.HasValue || dto.ContentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionTitle))
            {
                throw new BadRequestException(
                    "Question title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionDescription))
            {
                throw new BadRequestException(
                    "Question description is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramQuestionResponseDto>(
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
                        Name = "@question_title",
                        Value = dto.QuestionTitle.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question_description",
                        Value = dto.QuestionDescription.Trim()
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new Exception(
                    "Program question creation failed.");
            }

            return question;
        }

        public async Task<ProgramQuestionResponseDto> UpdateAsync(int programQuestionId,
            UpdateProgramQuestionDto dto)
        {
            if (programQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
            }

            if (!dto.ContentId.HasValue || dto.ContentId <= 0)
            {
                throw new BadRequestException(
                    "Content ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionTitle))
            {
                throw new BadRequestException(
                    "Question title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionDescription))
            {
                throw new BadRequestException(
                    "Question description is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramQuestionResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Update"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_question_id",
                        Value = programQuestionId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@content_id",
                        Value = dto.ContentId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question_title",
                        Value = dto.QuestionTitle.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@question_description",
                        Value = dto.QuestionDescription.Trim()
                    });

            var question = result.FirstOrDefault();

            if (question == null)
            {
                throw new NotFoundException(
                    $"Program question with ID {programQuestionId} was not found.");
            }

            return question;
        }

        public async Task<int> DeleteAsync(int programQuestionId)
        {
            if (programQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
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
                    Name = "@program_question_id",
                    Value = programQuestionId
                });
        }

        public async Task<int> RestoreAsync(int programQuestionId)
        {
            if (programQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
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
                    Name = "@program_question_id",
                    Value = programQuestionId
                });
        }
    }
}