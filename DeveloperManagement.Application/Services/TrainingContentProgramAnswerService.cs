using DeveloperManagement.Application.Contracts;
using DeveloperManagement.Application.DTOs.ProgramAnswer;
using DeveloperManagement.Application.Exceptions;
using DeveloperManagement.Application.Interfaces;

namespace DeveloperManagement.Application.Services
{
    public class TrainingContentProgramAnswerService: ITrainingContentProgramAnswerService
    {
        private readonly IGenericRepository _repository;
        private const string StoredProcedure = "erpsystem.sp_tblcontent_program_answers";

        public TrainingContentProgramAnswerService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProgramAnswerResponseDto>> GetAllAsync()
        {
            return await _repository.ExecuteQueryAsync<ProgramAnswerResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetAll"
                });
        }

        public async Task<ProgramAnswerResponseDto> GetByIdAsync(int programAnswerId)
        {
            if (programAnswerId <= 0)
            {
                throw new BadRequestException(
                    "Program answer ID must be greater than 0.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramAnswerResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetById"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_answer_id",
                        Value = programAnswerId
                    });

            var answer = result.FirstOrDefault();

            if (answer == null)
            {
                throw new NotFoundException(
                    $"Program answer with ID {programAnswerId} was not found.");
            }

            return answer;
        }

        public async Task<ProgramAnswerResponseDto> CreateAsync(CreateProgramAnswerDto dto)
        {
            if (!dto.ProgramQuestionId.HasValue ||
                dto.ProgramQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.ProgramAnswer))
            {
                throw new BadRequestException(
                    "Program answer is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ProgramDescription))
            {
                throw new BadRequestException(
                    "Program description is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramAnswerResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Insert"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_question_id",
                        Value = dto.ProgramQuestionId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_answer",
                        Value = dto.ProgramAnswer.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_description",
                        Value = dto.ProgramDescription.Trim()
                    });

            var answer = result.FirstOrDefault();

            if (answer == null)
            {
                throw new Exception(
                    "Program answer creation failed.");
            }

            return answer;
        }

        public async Task<ProgramAnswerResponseDto> UpdateAsync(int programAnswerId,
            UpdateProgramAnswerDto dto)
        {
            if (programAnswerId <= 0)
            {
                throw new BadRequestException(
                    "Program answer ID must be greater than 0.");
            }

            if (!dto.ProgramQuestionId.HasValue ||
                dto.ProgramQuestionId <= 0)
            {
                throw new BadRequestException(
                    "Program question ID must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.ProgramAnswer))
            {
                throw new BadRequestException(
                    "Program answer is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ProgramDescription))
            {
                throw new BadRequestException(
                    "Program description is required.");
            }

            var result =
                await _repository.ExecuteQueryAsync<ProgramAnswerResponseDto>(
                    StoredProcedure,
                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Update"
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_answer_id",
                        Value = programAnswerId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_question_id",
                        Value = dto.ProgramQuestionId
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_answer",
                        Value = dto.ProgramAnswer.Trim()
                    },
                    new StoredProcedureParameter
                    {
                        Name = "@program_description",
                        Value = dto.ProgramDescription.Trim()
                    });

            var answer = result.FirstOrDefault();

            if (answer == null)
            {
                throw new NotFoundException(
                    $"Program answer with ID {programAnswerId} was not found.");
            }

            return answer;
        }

        public async Task<int> DeleteAsync(int programAnswerId)
        {
            if (programAnswerId <= 0)
            {
                throw new BadRequestException(
                    "Program answer ID must be greater than 0.");
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
                    Name = "@program_answer_id",
                    Value = programAnswerId
                });
        }

        public async Task<int> RestoreAsync(int programAnswerId)
        {
            if (programAnswerId <= 0)
            {
                throw new BadRequestException(
                    "Program answer ID must be greater than 0.");
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
                    Name = "@program_answer_id",
                    Value = programAnswerId
                });
        }
    }
}