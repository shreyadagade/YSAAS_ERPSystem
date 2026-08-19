using DeveloperManagement.Application.DTOs.ProgramAnswer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingContentProgramAnswerService
    {
        Task<List<ProgramAnswerResponseDto>> GetAllAsync();

        Task<ProgramAnswerResponseDto> GetByIdAsync(int programAnswerId);

        Task<ProgramAnswerResponseDto> CreateAsync(CreateProgramAnswerDto dto);

        Task<ProgramAnswerResponseDto> UpdateAsync(int programAnswerId,UpdateProgramAnswerDto dto);

        Task<int> DeleteAsync(int programAnswerId);

        Task<int> RestoreAsync(int programAnswerId);
    }
}
