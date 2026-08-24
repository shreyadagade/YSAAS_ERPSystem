using StudentManagement.Application.DTOs.Qualification;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Interfaces.Services.Qualification
    {
        public interface IStudentQualificationService
        {
            Task<StudentQualificationResponseDto?> GetByIdAsync(
                int qualificationId);

            Task<IEnumerable<StudentQualificationResponseDto>>
                GetAllAsync();

            Task<StudentQualificationResponseDto> AddAsync(
                StudentQualificationRequestDto request);

            Task UpdateAsync(
                int qualificationId,
                StudentQualificationRequestDto request);

            Task DeleteAsync(
                int qualificationId);

            Task RestoreAsync(
                int qualificationId);
        }
    }
