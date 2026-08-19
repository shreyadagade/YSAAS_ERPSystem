using DeveloperManagement.Application.DTOs.ContentInterviewQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingContentInterviewQuestionService
    {
        Task<List<ContentInterviewQuestionResponseDto>> GetAllAsync();

        Task<ContentInterviewQuestionResponseDto> GetByIdAsync(int questionId);

        Task<ContentInterviewQuestionResponseDto> CreateAsync(
            CreateContentInterviewQuestionDto dto);

        Task<ContentInterviewQuestionResponseDto> UpdateAsync(int questionId,
            UpdateContentInterviewQuestionDto dto);

        Task<int> DeleteAsync(int questionId);

        Task<int> RestoreAsync(int questionId);
    }
}
