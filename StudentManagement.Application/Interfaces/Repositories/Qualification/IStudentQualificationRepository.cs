using StudentManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;



namespace StudentManagement.Application.Interfaces.Repositories.Qualification
    {
        public interface IStudentQualificationRepository
        {
            Task<StudentQualification?> GetByIdAsync(
                int qualificationId);

            Task<IEnumerable<StudentQualification>> GetAllAsync();

            Task<StudentQualification> AddAsync(
                StudentQualification qualification);

            Task UpdateAsync(
                StudentQualification qualification);

            Task DeleteAsync(
                int qualificationId);

            Task RestoreAsync(
                int qualificationId);
        }
    }
