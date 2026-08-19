using DeveloperManagement.Application.DTOs.ContentQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingContentQuestionService
    {
        Task<List<ContentQuestionResponseDto>> GetAllAsync();

        Task<ContentQuestionResponseDto> GetByIdAsync(int questionId);

        Task<ContentQuestionResponseDto> CreateAsync(CreateContentQuestionDto dto);

        Task<ContentQuestionResponseDto> UpdateAsync(int questionId,
            UpdateContentQuestionDto dto);

        Task<int> DeleteAsync(int questionId);

        Task<int> RestoreAsync(int questionId);
    }
}
