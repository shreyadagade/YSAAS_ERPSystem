using System;
using System.Collections.Generic;
using System.Text;

   namespace StudentManagement.Application.Interfaces.Repositories.Password
    {
        public interface IStudentPasswordRepository
        {
            Task<string?> GetPasswordHashByStudentIdAsync(
                int studentId);

            Task<bool> UpdatePasswordAsync(
                int studentId,
                string passwordHash);
        }
    }

