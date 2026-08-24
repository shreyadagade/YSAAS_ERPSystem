using StudentManagement.Application.DTOs.Qualification;
using StudentManagement.Application.Interfaces.Repositories.Qualification;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Qualification;
using StudentManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Services.Qualification
    {
        public class StudentQualificationService
            : IStudentQualificationService
        {
            private readonly IStudentQualificationRepository _repository;
            private readonly IStudentDetailsRepository _studentRepository;

            public StudentQualificationService(
                IStudentQualificationRepository repository,
                IStudentDetailsRepository studentRepository)
            {
                _repository = repository;
                _studentRepository = studentRepository;
            }

            // =====================================================
            // GET BY ID
            // =====================================================
            public async Task<StudentQualificationResponseDto?>
                GetByIdAsync(int qualificationId)
            {
                if (qualificationId <= 0)
                {
                    throw new ArgumentException(
                        "QualificationId must be greater than 0.");
                }

                var qualification =
                    await _repository.GetByIdAsync(
                        qualificationId);

                if (qualification == null)
                {
                    return null;
                }

                var student =
                    qualification.StudentId.HasValue
                        ? await _studentRepository.GetByIdAsync(
                            qualification.StudentId.Value)
                        : null;

                return MapToResponse(
                    qualification,
                    student?.StudentName);
            }

            // =====================================================
            // GET ALL
            // =====================================================
            public async Task<IEnumerable<StudentQualificationResponseDto>>
                GetAllAsync()
            {
                var qualifications =
                    await _repository.GetAllAsync();

                var response =
                    new List<StudentQualificationResponseDto>();

                foreach (var qualification in qualifications)
                {
                    string? studentName = null;

                    if (qualification.StudentId.HasValue)
                    {
                        var student =
                            await _studentRepository.GetByIdAsync(
                                qualification.StudentId.Value);

                        studentName =
                            student?.StudentName;
                    }

                    response.Add(
                        MapToResponse(
                            qualification,
                            studentName));
                }

                return response;
            }

            // =====================================================
            // CREATE
            // =====================================================
            public async Task<StudentQualificationResponseDto>
                AddAsync(
                    StudentQualificationRequestDto request)
            {
                if (request == null)
                {
                    throw new ArgumentException(
                        "Qualification data is required.");
                }

                ValidateQualification(request);

                // =================================================
                // CHECK STUDENT
                // =================================================

                if (!request.StudentId.HasValue)
                {
                    throw new ArgumentException(
                        "StudentId is required.");
                }

                var student =
                    await _studentRepository.GetByIdAsync(
                        request.StudentId.Value);

                if (student == null)
                {
                    throw new KeyNotFoundException(
                        "Student not found.");
                }

                // =================================================
                // CREATE ENTITY
                // =================================================

                var qualification =
                    new StudentQualification
                    {
                        StudentId =
                            request.StudentId,

                        Qualification =
                            request.Qualification,

                        PassingYear =
                            request.PassingYear,

                        University =
                            request.University,

                        Medium =
                            request.Medium,

                        Percentage =
                            request.Percentage
                    };

                // =================================================
                // INSERT
                // =================================================

                var result =
                    await _repository.AddAsync(
                        qualification);

                return MapToResponse(
                    result,
                    student.StudentName);
            }

            // =====================================================
            // UPDATE
            // =====================================================
            public async Task UpdateAsync(
                int qualificationId,
                StudentQualificationRequestDto request)
            {
                if (qualificationId <= 0)
                {
                    throw new ArgumentException(
                        "QualificationId must be greater than 0.");
                }

                if (request == null)
                {
                    throw new ArgumentException(
                        "Qualification data is required.");
                }

                ValidateQualification(request);

                if (!request.StudentId.HasValue)
                {
                    throw new ArgumentException(
                        "StudentId is required.");
                }

                var existing =
                    await _repository.GetByIdAsync(
                        qualificationId);

                if (existing == null)
                {
                    throw new KeyNotFoundException(
                        "Qualification not found.");
                }

                var student =
                    await _studentRepository.GetByIdAsync(
                        request.StudentId.Value);

                if (student == null)
                {
                    throw new KeyNotFoundException(
                        "Student not found.");
                }

                existing.StudentId =
                    request.StudentId;

                existing.Qualification =
                    request.Qualification;

                existing.PassingYear =
                    request.PassingYear;

                existing.University =
                    request.University;

                existing.Medium =
                    request.Medium;

                existing.Percentage =
                    request.Percentage;

                await _repository.UpdateAsync(
                    existing);
            }

            // =====================================================
            // DELETE
            // =====================================================
            public async Task DeleteAsync(
                int qualificationId)
            {
                if (qualificationId <= 0)
                {
                    throw new ArgumentException(
                        "QualificationId must be greater than 0.");
                }

                var existing =
                    await _repository.GetByIdAsync(
                        qualificationId);

                if (existing == null)
                {
                    throw new KeyNotFoundException(
                        "Qualification not found.");
                }

                await _repository.DeleteAsync(
                    qualificationId);
            }

            // =====================================================
            // RESTORE
            // =====================================================
            public async Task RestoreAsync(
                int qualificationId)
    
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "QualificationId must be greater than 0.");
            }

            await _repository.RestoreAsync(
                qualificationId);
        }

        // =====================================================
        // VALIDATION
        // =====================================================
        private static void ValidateQualification(
                StudentQualificationRequestDto request)
            {
                if (request.StudentId.HasValue &&
                    request.StudentId.Value <= 0)
                {
                    throw new ArgumentException(
                        "StudentId must be greater than 0.");
                }

                if (!string.IsNullOrWhiteSpace(
                    request.Qualification) &&
                    request.Qualification.Length > 100)
                {
                    throw new ArgumentException(
                        "Qualification cannot exceed 100 characters.");
                }

                if (request.PassingYear.HasValue &&
                    (request.PassingYear.Value < 1900 ||
                     request.PassingYear.Value > DateTime.Now.Year))
                {
                    throw new ArgumentException(
                        "Invalid passing year.");
                }

                if (!string.IsNullOrWhiteSpace(
                    request.University) &&
                    request.University.Length > 100)
                {
                    throw new ArgumentException(
                        "University cannot exceed 100 characters.");
                }

                if (!string.IsNullOrWhiteSpace(
                    request.Medium) &&
                    request.Medium.Length > 20)
                {
                    throw new ArgumentException(
                        "Medium cannot exceed 20 characters.");
                }

                if (request.Percentage.HasValue &&
                    (request.Percentage.Value < 0 ||
                     request.Percentage.Value > 100))
                {
                    throw new ArgumentException(
                        "Percentage must be between 0 and 100.");
                }
            }

            // =====================================================
            // ENTITY → RESPONSE DTO
            // =====================================================
            private static StudentQualificationResponseDto
                MapToResponse(
                    StudentQualification qualification,
                    string? studentName)
            {
                return new StudentQualificationResponseDto
                {
                    QualificationId =
                        qualification.QualificationId,

                    StudentId =
                        qualification.StudentId,

                    StudentName =
                        studentName,

                    Qualification =
                        qualification.Qualification,

                    PassingYear =
                        qualification.PassingYear,

                    University =
                        qualification.University,

                    Medium =
                        qualification.Medium,

                    Percentage =
                        qualification.Percentage
                };
            }
        }
    }
