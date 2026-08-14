using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentQualificationService : IStudentQualificationService
    {
        private readonly IStudentQualificationRepository _repository;

        public StudentQualificationService(
            IStudentQualificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentQualification?> GetByIdAsync(
            int qualificationId)
        {
            return await _repository.GetByIdAsync(qualificationId);
        }

        public async Task<IEnumerable<StudentQualification>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StudentQualification> AddAsync(
            StudentQualification qualification)
        {
            // Validation
            if (qualification.StudentId == null ||
                qualification.StudentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId is required.");
            }

            if (string.IsNullOrWhiteSpace(qualification.Qualification))
            {
                throw new ArgumentException(
                    "Qualification is required.");
            }

            if (qualification.Qualification.Length > 100)
            {
                throw new ArgumentException(
                    "Qualification cannot exceed 100 characters.");
            }

            if (qualification.PassingYear.HasValue)
            {
                if (qualification.PassingYear < 1900 ||
                    qualification.PassingYear > DateTime.Now.Year)
                {
                    throw new ArgumentException(
                        "Passing year is invalid.");
                }
            }

            if (string.IsNullOrWhiteSpace(qualification.University))
            {
                throw new ArgumentException(
                    "University is required.");
            }

            if (qualification.University.Length > 100)
            {
                throw new ArgumentException(
                    "University cannot exceed 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(qualification.Medium) &&
                qualification.Medium.Length > 20)
            {
                throw new ArgumentException(
                    "Medium cannot exceed 20 characters.");
            }

            if (qualification.Percentage.HasValue)
            {
                if (qualification.Percentage < 0 ||
                    qualification.Percentage > 100)
                {
                    throw new ArgumentException(
                        "Percentage must be between 0 and 100.");
                }
            }

            return await _repository.AddAsync(qualification);
        }

        public async Task UpdateAsync(
            StudentQualification qualification)
        {
            if (qualification.QualificationId <= 0)
            {
                throw new ArgumentException(
                    "QualificationId is required.");
            }

            if (qualification.StudentId == null ||
                qualification.StudentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId is required.");
            }

            if (string.IsNullOrWhiteSpace(qualification.Qualification))
            {
                throw new ArgumentException(
                    "Qualification is required.");
            }

            if (qualification.Qualification.Length > 100)
            {
                throw new ArgumentException(
                    "Qualification cannot exceed 100 characters.");
            }

            if (qualification.PassingYear.HasValue)
            {
                if (qualification.PassingYear < 1900 ||
                    qualification.PassingYear > DateTime.Now.Year)
                {
                    throw new ArgumentException(
                        "Passing year is invalid.");
                }
            }

            if (string.IsNullOrWhiteSpace(qualification.University))
            {
                throw new ArgumentException(
                    "University is required.");
            }

            if (qualification.University.Length > 100)
            {
                throw new ArgumentException(
                    "University cannot exceed 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(qualification.Medium) &&
                qualification.Medium.Length > 20)
            {
                throw new ArgumentException(
                    "Medium cannot exceed 20 characters.");
            }

            if (qualification.Percentage.HasValue)
            {
                if (qualification.Percentage < 0 ||
                    qualification.Percentage > 100)
                {
                    throw new ArgumentException(
                        "Percentage must be between 0 and 100.");
                }
            }

            await _repository.UpdateAsync(qualification);
        }

        public async Task DeleteAsync(int qualificationId)
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid QualificationId.");
            }

            await _repository.DeleteAsync(qualificationId);
        }

        public async Task RestoreAsync(int qualificationId)
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid QualificationId.");
            }

            await _repository.RestoreAsync(qualificationId);
        }
    }
}