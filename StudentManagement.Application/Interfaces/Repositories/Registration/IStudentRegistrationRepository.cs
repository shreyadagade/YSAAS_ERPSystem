using StudentManagement.Domain.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.Interfaces.Repositories.Registration
{
          public interface IStudentRegistrationRepository
        {
            // Get registration by ID
            Task<StudentRegistration?> GetByIdAsync(
                int registrationId);

            // Get all registrations
            Task<IEnumerable<StudentRegistration>> GetAllAsync();

            // Create registration
            Task<StudentRegistration> AddAsync(
                StudentRegistration registration);

            // Update registration
            Task UpdateAsync(
                StudentRegistration registration);

            // Soft delete registration
            Task DeleteAsync(
                int registrationId);

            // Restore registration
            Task RestoreAsync(
                int registrationId);
        }
    }

