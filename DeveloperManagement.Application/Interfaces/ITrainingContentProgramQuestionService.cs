using DeveloperManagement.Application.DTOs.ProgramQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingContentProgramQuestionService
    {
        Task<ProgramQuestionResponseDto> CreateAsync(CreateProgramQuestionDto dto);

        Task<ProgramQuestionResponseDto> UpdateAsync(int programQuestionId,UpdateProgramQuestionDto dto);

        Task<int> DeleteAsync(int programQuestionId);

        Task<int> RestoreAsync(int programQuestionId);

        Task<List<ProgramQuestionResponseDto>> GetAllAsync();

        Task<ProgramQuestionResponseDto> GetByIdAsync(int programQuestionId);
    }
}
